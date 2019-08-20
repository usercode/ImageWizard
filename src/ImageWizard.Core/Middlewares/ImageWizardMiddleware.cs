using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.ImageLoaders;
using ImageWizard.ImageStorages;
using ImageWizard.Services.Types;
using ImageWizard.Settings;
using ImageWizard.SharedContract;
using ImageWizard.SharedContract.FilterTypes;
using ImageWizard.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using System.Globalization;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Types;
using System.Threading;
using System.Security.Cryptography;
using ImageWizard.Core;
using ImageWizard.Core.Middlewares;

namespace ImageWizard.Middlewares
{
    /// <summary>
    /// ImageWizardMiddleware
    /// </summary>
    public class ImageWizardMiddleware
    {
        private readonly RequestDelegate _next;

        public ImageWizardMiddleware(
            RequestDelegate next,
            IOptions<ImageWizardSettings> settings,
            ILogger<ImageWizardMiddleware> logger,
            FilterManager filterManager,
            ImageLoaderManager imageLoaderManager
            )
        {
            Settings = settings;
            FilterManager = filterManager;
            ImageLoaderManager = imageLoaderManager;
            Logger = logger;

            _next = next;

            CryptoService = new CryptoService(Settings.Value.Key);

            UrlRegex = new Regex($@"^{Settings.Value.BasePath.Value}/(?<signature>[a-z0-9-_]+)/(?<path>(?<filter>[a-z]+\([a-z0-9,.]*\)/)*(?<loaderType>[a-z]+)/(?<loaderSource>.*))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        private IOptions<ImageWizardSettings> Settings { get; }

        /// <summary>
        /// FilterManager
        /// </summary>
        private FilterManager FilterManager { get; }

        /// <summary>
        /// ImageLoaderManager
        /// </summary>
        private ImageLoaderManager ImageLoaderManager { get; }

        /// <summary>
        /// CryptoService
        /// </summary>
        private CryptoService CryptoService { get; }

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
            if (context.Request.Path.StartsWithSegments(Settings.Value.BasePath) == false)
            {
                await _next(context);

                return;
            }

            string path = context.Request.Path.Value;

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
            string[] url_filters = match.Groups["filter"].Captures.OfType<Capture>()
                                                                            .Select(x => x.Value)
                                                                            .Select(x => x.Substring(0, x.Length -1)) //remove "/"
                                                                            .ToArray();

            string signature = CryptoService.Encrypt(url_path);

            //check unsafe keyword or signature
            if ((Settings.Value.AllowUnsafeUrl && url_signature == "unsafe") == false
                &&
                (signature == url_signature) == false)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("signature is not valid!");

                return;
            }

            IImageCache imageCache = context.RequestServices.GetService<IImageCache>();

            if(imageCache == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Image cache not found");

                return;
            }

            //try to get cached image
            ICachedImage cachedImage = await imageCache.ReadAsync(signature);

            //no cached image found?
            if (cachedImage == null)
            {
                Logger.LogTrace("Create cached image");

                Type imageLoaderType = ImageLoaderManager.Get(url_loaderType);
                IImageLoader loader = (IImageLoader)context.RequestServices.GetService(imageLoaderType);

                if (loader == null)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("image loader not found: " + url_loaderType);

                    return;
                }

                Logger.LogTrace("Get original image: " + url_loaderSource);

                //download image
                OriginalImage originalImage = await loader.GetAsync(url_loaderSource);

                IImageFormat targetFormat = ImageFormatHelper.Parse(originalImage.MimeType);

                byte[] transformedImageData;

                //create metadata
                ImageMetadata imageMetadata = new ImageMetadata()
                {
                    Cache = originalImage.CacheSettings,
                    CreatedAt = DateTime.UtcNow,
                    Signature = signature,
                    Filters = url_filters,
                    LoaderSource = url_loaderSource,
                    LoaderType = url_loaderType,
                    MimeType = targetFormat.MimeType,
                    DPR = null
                };

                //skip svg
                if (targetFormat is SvgFormat)
                {
                    Logger.LogTrace("SVG file: skip filters");

                    transformedImageData = originalImage.Data;
                }
                else
                {
                    //load image
                    using (Image<Rgba32> image = Image.Load(originalImage.Data))
                    {
                        FilterContext filterContext = new FilterContext(Settings.Value, image, targetFormat);

                        //use clint hints?
                        if (Settings.Value.UseClintHints)
                        {
                            //check DPR value from request
                            string ch_dpr = context.Request.Headers["DPR"].FirstOrDefault();
                            string ch_width = context.Request.Headers["Width"].FirstOrDefault();
                            string ch_viewportWidth = context.Request.Headers["Viewport-Width"].FirstOrDefault();

                            if (ch_dpr != null)
                            {
                                filterContext.ClientHints.DPR = double.Parse(ch_dpr, CultureInfo.InvariantCulture);
                                filterContext.DPR = filterContext.ClientHints.DPR;
                            }

                            if (ch_width != null)
                            {
                                filterContext.ClientHints.Width = int.Parse(ch_width, CultureInfo.InvariantCulture);
                            }

                            if (ch_viewportWidth != null)
                            {
                                filterContext.ClientHints.ViewportWidth = int.Parse(ch_viewportWidth, CultureInfo.InvariantCulture);
                            }
                        }

                        //execute filters
                        foreach (string filter in url_filters)
                        {
                            //find and execute filter
                            IFilterAction foundFilter = FilterManager.FilterActions.FirstOrDefault(x => x.TryExecute(filter, filterContext));

                            if (foundFilter != null)
                            {
                                Logger.LogTrace("Filter executed: " + filter);
                            }
                            else
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsync($"filter was not found: {filter}");

                                return;
                            }
                        }

                        //check max width and height
                        bool change = false;

                        int width = image.Width;
                        int height = image.Height;

                        if (Settings.Value.ImageMaxWidth != null && width > Settings.Value.ImageMaxWidth)
                        {
                            change = true;
                            width = Settings.Value.ImageMaxWidth.Value;
                        }

                        if (Settings.Value.ImageMaxHeight != null && height > Settings.Value.ImageMaxHeight)
                        {
                            change = true;
                            height = Settings.Value.ImageMaxHeight.Value;
                        }

                        if (change == true)
                        {
                            new ResizeFilter().Execute(width, height, ResizeMode.Max, filterContext);
                        }

                        MemoryStream mem = new MemoryStream();

                        //generate image
                        filterContext.ImageFormat.SaveImage(image, mem);

                        transformedImageData = mem.ToArray();

                        //update some metadata
                        imageMetadata.DPR = filterContext.DPR;
                        imageMetadata.MimeType = filterContext.ImageFormat.MimeType;
                        imageMetadata.NoImageCache = filterContext.NoImageCache;
                    }
                }

                Logger.LogTrace("Save new cached image");

                //create hash of cached image
                SHA256 sha = SHA256.Create();
                byte[] hash = sha.ComputeHash(transformedImageData);

                imageMetadata.Hash = hash.ToHexcode();
                imageMetadata.FileLength = transformedImageData.Length;

                //disable cache?
                if (imageMetadata.NoImageCache == false)
                {
                    //save cached image
                    await imageCache.WriteAsync(signature, imageMetadata, transformedImageData);
                }

                cachedImage = new CachedImage(imageMetadata, () => Task.FromResult<Stream>(new MemoryStream(transformedImageData)));
            }
            else
            {
                Logger.LogTrace("Cached image found");

                //check ETag from request
                if (Settings.Value.UseETag)
                {
                    bool? isValid = context.Request.GetTypedHeaders().IfNoneMatch?.Any(x => x.Tag == $"\"{cachedImage.Metadata.Hash}\"");

                    if (isValid == true)
                    {
                        Logger.LogTrace("Operation completed: 304 Not modified");

                        context.Response.StatusCode = StatusCodes.Status304NotModified;

                        return;
                    }
                }
            }

            //set cache control header
            if (Settings.Value.CacheControl.IsEnabled)
            {
                context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                {
                    Public = Settings.Value.CacheControl.Public,
                    MustRevalidate = Settings.Value.CacheControl.MustRevalidate,
                    MaxAge = Settings.Value.CacheControl.MaxAge,
                    NoCache = Settings.Value.CacheControl.NoCache,
                    NoStore = Settings.Value.CacheControl.NoStore
                };
            }
            
            //set ETag header
            if (Settings.Value.UseETag)
            {
                context.Response.GetTypedHeaders().ETag = new EntityTagHeaderValue($"\"{cachedImage.Metadata.Hash}\"");
            }

            //DPR
            if (cachedImage.Metadata.DPR != null)
            {
                context.Response.Headers.Add("Content-DPR", cachedImage.Metadata.DPR.Value.ToString(CultureInfo.InvariantCulture));
                context.Response.Headers.Add("Vary", "DPR");
            }

            //send response stream
            using (Stream stream = await cachedImage.OpenReadAsync())
            {
                context.Response.ContentLength = stream.Length;
                context.Response.ContentType = cachedImage.Metadata.MimeType;

                interceptors.Foreach(x => x.OnResponseSending(context.Response, cachedImage));

                await stream.CopyToAsync(context.Response.Body);
            }

            Logger.LogTrace("Operation completed");
        }
    }
}
