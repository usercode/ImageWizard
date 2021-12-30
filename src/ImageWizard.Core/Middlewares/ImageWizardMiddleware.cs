using ImageWizard.Caches;
using ImageWizard.Core;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.Core.StreamPooling;
using ImageWizard.Core.Types;
using ImageWizard.ImageLoaders;
using ImageWizard.Metadatas;
using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using ImageWizard.Services.Types;
using ImageWizard.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Middlewares
{
    /// <summary>
    /// ImageWizardMiddleware
    /// </summary>
    class ImageWizardMiddleware
    {
        public ImageWizardMiddleware(
                    RequestDelegate next,
                    IOptions<ImageWizardOptions> options,
                    ILogger<ImageWizardMiddleware> logger,
                    ImageWizardBuilder builder                    
                    )
        {
            Options = options.Value;
            Builder = builder;
            Logger = logger;
        }

        /// <summary>
        /// Options
        /// </summary>
        private ImageWizardOptions Options { get; }

        /// <summary>
        /// ImageLoaderManager
        /// </summary>
        private ImageWizardBuilder Builder { get; }

        /// <summary>
        /// Logger
        /// </summary>
        private ILogger<ImageWizardMiddleware> Logger { get; }

        public async Task InvokeAsync(HttpContext context)
        {
            string? path = (string?)context.GetRouteValue("imagePath");

            if (path == null)
            {
                throw new Exception("path is not available.");
            }

            if (context.Request.QueryString.HasValue)
            {
                path += context.Request.QueryString.Value;
            }

            //parse url
            if (ImageWizardUrl.TryParse(path, out ImageWizardUrl url) == false)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                return;
            }

            IEnumerable<IImageWizardInterceptor> interceptors = context.RequestServices.GetServices<IImageWizardInterceptor>();

            //unsafe url?
            if (Options.AllowUnsafeUrl && url.IsUnsafeUrl)
            {
                Logger.LogTrace("unsafe request");
            }
            else
            {
                //check signature
                if (url.IsSignatureValid(Options.Key))
                {
                    Logger.LogTrace("signature is valid");
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;

                    await context.Response.WriteAsync("signature is not valid!");

                    Logger.LogTrace("signature is not valid");

                    interceptors.Foreach(x => x.OnFailedSignature());

                    return;
                }
            }

            //get image loader
            Type loaderType = Builder.ImageLoaderManager.Get(url.LoaderType);
            IDataLoader? loader = (IDataLoader?)context.RequestServices.GetService(loaderType);

            if (loader == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Data loader not found: " + url.LoaderType);

                return;
            }

            string url_path_with_headers = url.Path;

            //get compatible mime types by the accept header
            IEnumerable<string> acceptMimeTypes = context.Request.GetTypedHeaders().Accept
                                                                        .Where(x => x.MatchesAllSubTypes == false)
                                                                        .Select(x => x.MediaType.Value)
                                                                        //filter unknown mime types
                                                                        .Where(x => Builder.GetAllMimeTypes().Contains(x))
                                                                        .ToList();

            //use accept header?
            if (Options.UseAcceptHeader)
            {
                if (acceptMimeTypes.Any())
                {
                    url_path_with_headers += $"+Accept={string.Join(",", acceptMimeTypes)}";
                }
            }

            //generate image key
            byte[] keyInBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(url_path_with_headers));
            string key = Convert.ToHexString(keyInBytes);

            //try to get the cached image
            ICachedData? cachedData = null;

            //get image cache
            ICache? cache = context.RequestServices.GetService<ICache>();

            if (cache != null)
            {
                cachedData = await cache.ReadAsync(key);
            }
            else
            {
                Logger.LogWarning("There is no cache available.");
            }

            bool createCachedData = false;
            //no cached image found?
            if (cachedData == null)
            {
                createCachedData = true;
            }
            else //use existing cached image
            {
                switch (loader.RefreshMode)
                {
                    case DataLoaderRefreshMode.None:
                        createCachedData = false;
                        break;

                    case DataLoaderRefreshMode.EveryTime:
                        createCachedData = true;
                        break;

                    case DataLoaderRefreshMode.UseRemoteCacheControl:
                        if (cachedData.Metadata.Cache.NoStore == true
                            || cachedData.Metadata.Cache.NoCache == true
                            || (cachedData.Metadata.Cache.Expires != null && cachedData.Metadata.Cache.Expires < DateTime.UtcNow)
                            //(cachedImage.Metadata.Cache.LastModified != null)
                            )
                        {
                            createCachedData = true;
                        }
                        break;

                    default:
                        throw new Exception("unknown refresh mode");
                }
            }

            //create cached image
            if (createCachedData == true)
            {
                Logger.LogTrace("Create cached data");

                //get original image
                OriginalData? originalData = await loader.GetAsync(url.LoaderSource, cachedData);

                if (originalData != null) //is there a new version of original image?
                {
                    ClientHints clientHints = context.Request.GetClientHints(Options.AllowedDPR);

                    ProcessingPipelineContext processingContext = new ProcessingPipelineContext(
                                                                context.RequestServices.GetRequiredService<IStreamPool>(),
                                                                 new ImageResult(originalData.Data, originalData.MimeType),
                                                                 clientHints,
                                                                 Options,
                                                                 acceptMimeTypes,
                                                                 url.Filters);

                    do
                    {
                        //find processing pipeline by mime type
                        Type processingPipelineType = Builder.GetPipeline(processingContext.Result.MimeType);
                        IProcessingPipeline? processingPipeline = (IProcessingPipeline?)context.RequestServices.GetService(processingPipelineType);

                        if (processingPipeline == null)
                        {
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync("Processing pipeline was not found: " + processingContext.Result.MimeType);

                            return;
                        }

                        Logger.LogTrace($"Start pipline: {processingPipelineType.Name}");

                        //start processing
                        processingContext.Result = await processingPipeline.StartAsync(processingContext);

                    } while (processingContext.UrlFilters.Count > 0);

                    Logger.LogTrace("Save new cached image");

                    //create hash of cached image
                    byte[] hash = SHA256.Create().ComputeHash(processingContext.Result.Data);

                    //create metadata
                    Metadata imageMetadata = new Metadata()
                    {
                        Cache = originalData.Cache,
                        Created = DateTime.UtcNow,
                        Key = key,
                        Filters = url.Filters,
                        LoaderSource = url.LoaderSource,
                        LoaderType = url.LoaderType,
                        Hash = Convert.ToHexString(hash),
                        MimeType = processingContext.Result.MimeType,
                        DPR = processingContext.Result.DPR,
                        FileLength = processingContext.Result.Data.Length,
                        Width = processingContext.Result.Width,
                        Height = processingContext.Result.Height
                    };

                    cachedData = new CachedData(imageMetadata, () => Task.FromResult<Stream>(new MemoryStream(processingContext.Result.Data)));

                    if (cache != null)
                    {
                        //disable cache?
                        if ((loader.RefreshMode == DataLoaderRefreshMode.UseRemoteCacheControl && cachedData.Metadata.Cache.NoStore) == false)
                        {
                            //save cached image
                            await cache.WriteAsync(key, cachedData);
                        }
                    }

                    interceptors.Foreach(x => x.OnCachedImageCreated(cachedData));
                }
            }

            if (cachedData == null)
            {
                throw new ArgumentNullException(nameof(cachedData));
            }

            //send "304-NotModified" if etag is valid
            if (Options.UseETag)
            {
                bool? isValid = context.Request.GetTypedHeaders().IfNoneMatch?.Any(x => x.Tag == $"\"{cachedData.Metadata.Hash}\"");

                if (isValid == true)
                {
                    Logger.LogTrace("Operation completed: 304 Not modified");

                    context.Response.StatusCode = StatusCodes.Status304NotModified;

                    return;
                }
            }

            IList<string> varyHeader = new List<string>();

            //use accept header
            if (Options.UseAcceptHeader)
            {
                varyHeader.Add(HeaderNames.Accept);
            }

            //use client hints
            if (Options.UseClintHints)
            {
                varyHeader.Add(ClientHints.DPRHeader);
                varyHeader.Add(ClientHints.WidthHeader);
                varyHeader.Add(ClientHints.ViewportWidthHeader);
            }

            //set cache control header
            if (Options.CacheControl.IsEnabled)
            {
                context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                {
                    Public = Options.CacheControl.Public,
                    MustRevalidate = Options.CacheControl.MustRevalidate,
                    MaxAge = Options.CacheControl.MaxAge,
                    NoCache = Options.CacheControl.NoCache,
                    NoStore = Options.CacheControl.NoStore
                };
            }

            //set ETag header
            if (Options.UseETag)
            {
                context.Response.GetTypedHeaders().ETag = new EntityTagHeaderValue($"\"{cachedData.Metadata.Hash}\"");
            }

            //DPR
            if (cachedData.Metadata.DPR != null)
            {
                context.Response.Headers.Add("Content-DPR", cachedData.Metadata.DPR.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (varyHeader.Count > 0)
            {
                context.Response.Headers.Add(HeaderNames.Vary, string.Join(", ", varyHeader));
            }

            //context.Response.Headers[HeaderNames.AcceptRanges] = "bytes";
            context.Response.ContentLength = cachedData.Metadata.FileLength;
            context.Response.ContentType = cachedData.Metadata.MimeType;

            //is HEAD request?
            bool isHeadRequst = context.Request.Method.Equals("HEAD", StringComparison.OrdinalIgnoreCase);

            if (isHeadRequst == false)
            {
                interceptors.Foreach(x => x.OnResponseSending(context.Response, cachedData));

                //send response stream
                using (Stream stream = await cachedData.OpenReadAsync())
                {
                    await stream.CopyToAsync(context.Response.Body);
                }

                interceptors.Foreach(x => x.OnResponseCompleted(cachedData));
            }

            Logger.LogTrace("Operation completed");
        }
    }
}
