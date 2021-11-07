using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.ImageLoaders;
using ImageWizard.ImageStorages;
using ImageWizard.Services.Types;
using ImageWizard.Settings;
using ImageWizard.Types;
using ImageWizard.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.Middlewares
{
    /// <summary>
    /// ImageWizardMiddleware
    /// </summary>
    class ImageWizardMiddleware
    {
        private readonly RequestDelegate _next;

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

            _next = next;

            const string signature = @"[a-z0-9-_]+";
            const string filter = @"[a-z]+\([^)]*\)";
            const string loaderType = @"[a-z]+";
            const string loaderSource = @".*";
            
            UrlRegex = new Regex($@"^(?<signature>{signature})/(?<path>(?<filter>{filter}/)*(?<loaderType>{loaderType})/(?<loaderSource>{loaderSource}))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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

        /// <summary>
        /// UrlRegex
        /// </summary>
        private Regex UrlRegex { get; }

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

            Match match = UrlRegex.Match(path);

            if (match.Success == false)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                return;
            }

            IEnumerable<IImageWizardInterceptor> interceptors = context.RequestServices.GetServices<IImageWizardInterceptor>();

            string url_signature = match.Groups["signature"].Value;
            string url_path = match.Groups["path"].Value;
            string url_loaderSource = match.Groups["loaderSource"].Value;
            string url_loaderType = match.Groups["loaderType"].Value;
            IList<string> url_filters = match.Groups["filter"].Captures.OfType<Capture>()
                                                                            .Select(x => x.Value)
                                                                            .Select(x => x[0..^1]) //remove "/"
                                                                            .ToList();

            //unsafe url?
            if (Options.AllowUnsafeUrl && url_signature == "unsafe")
            {
                Logger.LogTrace("unsafe request");
            }
            else
            {
                //check signature
                string signature = new CryptoService(Options.Key).Encrypt(url_path);

                if (signature == url_signature)
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
            Type imageLoaderType = Builder.ImageLoaderManager.Get(url_loaderType);
            IImageLoader? loader = (IImageLoader?)context.RequestServices.GetService(imageLoaderType);

            if (loader == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Image loader not found: " + url_loaderType);

                return;
            }

            string url_Path_with_headers = url_path;

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
                    url_Path_with_headers += $"+Accept={string.Join(",", acceptMimeTypes)}";
                }
            }

            //generate image key
            byte[] keyInBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(url_Path_with_headers));
            string key = Convert.ToHexString(keyInBytes);

            //try to get the cached image
            ICachedImage? cachedImage = null;

            //get image cache
            IImageCache? imageCache = context.RequestServices.GetService<IImageCache>();

            if (imageCache != null)
            {
                cachedImage = await imageCache.ReadAsync(key);
            }
            else
            {
                Logger.LogWarning("There is no image cache available.");
            }
            
            bool createCachedImage = false;
            
            //no cached image found?
            if (cachedImage == null)
            {
                createCachedImage = true;
            }
            else //use existing cached image
            {
                switch (loader.RefreshMode)
                {
                    case ImageLoaderRefreshMode.None:
                        createCachedImage = false;
                        break;

                    case ImageLoaderRefreshMode.EveryTime:
                        createCachedImage = true;
                        break;

                    case ImageLoaderRefreshMode.UseRemoteCacheControl:
                        if(cachedImage.Metadata.Cache.NoStore == true
                            || cachedImage.Metadata.Cache.NoCache == true
                            || (cachedImage.Metadata.Cache.Expires != null && cachedImage.Metadata.Cache.Expires < DateTime.UtcNow)
                            //(cachedImage.Metadata.Cache.LastModified != null)
                            )
                        {
                            createCachedImage = true;
                        }
                        break;

                    default:
                        throw new Exception("unknown refresh mode");
                }
            }

            //create cached image
            if (createCachedImage == true)
            {
                Logger.LogTrace("Create cached image");

                //get original image
                OriginalData? originalData = await loader.GetAsync(url_loaderSource, cachedImage);

                if (originalData != null) //is there a new version of original image?
                {
                    ClientHints clientHints = context.Request.GetClientHints(Options.AllowedDPR);

                    ProcessingPipelineContext processingContext = new ProcessingPipelineContext(
                                                                 new ImageResult(originalData.Data, originalData.MimeType),
                                                                 clientHints,
                                                                 Options,
                                                                 acceptMimeTypes,
                                                                 url_filters);

                    while (processingContext.UrlFilters.Count > 0)
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

                        //start processing
                        processingContext.Result = await processingPipeline.StartAsync(processingContext);
                    }
                    
                    Logger.LogTrace("Save new cached image");

                    //create hash of cached image
                    byte[] hash = SHA256.Create().ComputeHash(processingContext.Result.Data);

                    //create metadata
                    ImageMetadata imageMetadata = new ImageMetadata()
                    {
                        Cache = originalData.Cache,
                        Created = DateTime.UtcNow,
                        Key = key,
                        Filters = url_filters,
                        LoaderSource = url_loaderSource,
                        LoaderType = url_loaderType,
                        Hash = Convert.ToHexString(hash),
                        MimeType = processingContext.Result.MimeType,
                        DPR = processingContext.Result.DPR,
                        FileLength = processingContext.Result.Data.Length,                        
                        Width = processingContext.Result.Width,
                        Height = processingContext.Result.Height
                    };

                    cachedImage = new CachedImage(imageMetadata, () => Task.FromResult<Stream>(new MemoryStream(processingContext.Result.Data)));

                    if (imageCache != null)
                    {
                        //disable cache?
                        if (processingContext.DisableCache == false
                            && (loader.RefreshMode == ImageLoaderRefreshMode.UseRemoteCacheControl && cachedImage.Metadata.Cache.NoStore) == false)
                        {
                            //save cached image
                            await imageCache.WriteAsync(key, cachedImage);
                        }
                    }

                    interceptors.Foreach(x => x.OnCachedImageCreated(cachedImage));
                }
            }

            if (cachedImage == null)
            {
                throw new ArgumentNullException(nameof(cachedImage));
            }

            //send "304-NotModified" if etag is valid
            if (Options.UseETag)
            {
                bool? isValid = context.Request.GetTypedHeaders().IfNoneMatch?.Any(x => x.Tag == $"\"{cachedImage.Metadata.Hash}\"");

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
                context.Response.GetTypedHeaders().ETag = new EntityTagHeaderValue($"\"{cachedImage.Metadata.Hash}\"");
            }

            //DPR
            if (cachedImage.Metadata.DPR != null)
            {
                context.Response.Headers.Add("Content-DPR", cachedImage.Metadata.DPR.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (varyHeader.Count > 0)
            {
                context.Response.Headers.Add(HeaderNames.Vary, string.Join(", ", varyHeader));
            }

            //context.Response.Headers[HeaderNames.AcceptRanges] = "bytes";
            context.Response.ContentLength = cachedImage.Metadata.FileLength;
            context.Response.ContentType = cachedImage.Metadata.MimeType;

            //is HEAD request?
            bool isHeadRequst = context.Request.Method.Equals("HEAD", StringComparison.OrdinalIgnoreCase);

            if (isHeadRequst == false)
            {
                interceptors.Foreach(x => x.OnResponseSending(context.Response, cachedImage));

                //send response stream
                using (Stream stream = await cachedImage.OpenReadAsync())
                {
                    await stream.CopyToAsync(context.Response.Body);
                }

                interceptors.Foreach(x => x.OnResponseCompleted(cachedImage));
            }

            Logger.LogTrace("Operation completed");
        }
    }
}
