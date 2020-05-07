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

            UrlRegex = new Regex($@"^(?<signature>[a-z0-9-_]+)/(?<path>(?<filter>[a-z]+\([a-z0-9,.=']*\)/)*(?<loaderType>[a-z]+)/(?<loaderSource>.*))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
            string path = WebUtility.UrlDecode((string)context.GetRouteValue("imagePath"));

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

            //get image cache
            IImageCache imageCache = context.RequestServices.GetService<IImageCache>();

            if (imageCache == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Image cache not found");

                return;
            }

            //get image loader
            Type imageLoaderType = Builder.ImageLoaderManager.Get(url_loaderType);
            IImageLoader loader = (IImageLoader)context.RequestServices.GetService(imageLoaderType);

            if (loader == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("image loader not found: " + url_loaderType);

                return;
            }

            string url_newPath = url_path;

            ClientHints clientHints = new ClientHints(Options.AllowedDPR);

            //use clint hints?
            if (Options.UseClintHints)
            {
                //check DPR value from request
                string ch_dpr = context.Request.Headers["DPR"].FirstOrDefault();
                string ch_width = context.Request.Headers["Width"].FirstOrDefault();
                string ch_viewportWidth = context.Request.Headers["Viewport-Width"].FirstOrDefault();

                List<string> extraMethods = new List<string>();

                if (ch_dpr != null)
                {
                   clientHints.DPR = double.Parse(ch_dpr, CultureInfo.InvariantCulture);

                    string method = $"dpr({clientHints.DPR.Value.ToString("0.0", CultureInfo.InvariantCulture)})";
                    extraMethods.Add(method);
                }

                if (ch_width != null)
                {
                    clientHints.Width = int.Parse(ch_width, CultureInfo.InvariantCulture);

                    //extraMethods.Insert(0, $"resize({clientHints.Width},0)");
                }

                if (ch_viewportWidth != null)
                {
                    clientHints.ViewportWidth = int.Parse(ch_viewportWidth, CultureInfo.InvariantCulture);

                    //extraMethods.Insert(0, $"resize({clientHints.ViewportWidth},0)");
                }

                if (extraMethods.Any())
                {
                    //rebuild path
                    StringBuilder url = new StringBuilder(url_path);

                    foreach(string method in extraMethods)
                    {
                        url.Insert(0, "/");
                        url.Insert(0, method);
                    }

                    url_newPath = url.ToString();
                }
            }

            //generate image key
            byte[] keyInBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(url_newPath));
            string key = keyInBytes.ToHexcode();

            //try to get the cached image
            ICachedImage cachedImage = await imageCache.ReadAsync(key);
            
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
                OriginalImage originalImage = await loader.GetAsync(url_loaderSource, cachedImage);

                if (originalImage != null) //is there a new version of original image?
                {
                    var processingContext = new ProcessingPipelineContext(
                                                                 new CurrentImage(originalImage.MimeType, originalImage.Data, null, null, null),
                                                                 clientHints,
                                                                 Options,
                                                                 url_filters);

                    //find processing pipeline by mime type
                    Type processingPipelineType = Builder.GetPipeline(processingContext.CurrentImage.MimeType);
                    IProcessingPipeline processingPipeline = (IProcessingPipeline)context.RequestServices.GetService(processingPipelineType);

                    if (processingPipeline == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync("Processing pipeline not found: " + processingContext.CurrentImage.MimeType);

                        return;
                    }

                    //start processing
                    await processingPipeline.StartAsync(processingContext);
                    
                    Logger.LogTrace("Save new cached image");

                    //create hash of cached image
                    byte[] hash = SHA256.Create().ComputeHash(processingContext.CurrentImage.Data);

                    //create metadata
                    ImageMetadata imageMetadata = new ImageMetadata()
                    {
                        Cache = originalImage.Cache,
                        Created = DateTime.UtcNow,
                        Key = key,
                        Filters = url_filters,
                        LoaderSource = url_loaderSource,
                        LoaderType = url_loaderType,
                        Hash = hash.ToHexcode(),
                        MimeType = processingContext.CurrentImage.MimeType,
                        DPR = processingContext.CurrentImage.DPR,
                        FileLength = processingContext.CurrentImage.Data.Length,                        
                        Width = processingContext.CurrentImage.Width,
                        Height = processingContext.CurrentImage.Height
                    };

                    cachedImage = new CachedImage(imageMetadata, () => Task.FromResult<Stream>(new MemoryStream(processingContext.CurrentImage.Data)));

                    //disable cache?
                    if (processingContext.DisableCache == false
                        && (loader.RefreshMode == ImageLoaderRefreshMode.UseRemoteCacheControl && cachedImage.Metadata.Cache.NoStore) == false)
                    {
                        //save cached image
                        await imageCache.WriteAsync(key, cachedImage);
                    }

                    interceptors.Foreach(x => x.OnCachedImageCreated(cachedImage));
                }
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
                context.Response.Headers.Add(HeaderNames.Vary, "DPR");
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
