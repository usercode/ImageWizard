using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Types;
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
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

            UrlRegex = new Regex($@"^(?<signature>[a-z0-9-_]+)/(?<path>(?<filter>[a-z]+\([a-z0-9,.=']*\)/)*(?<loaderType>[a-z]+)/(?<loaderSource>.*))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
        /// Logger
        /// </summary>
        private ILogger<ImageWizardMiddleware> Logger { get; }

        /// <summary>
        /// UrlRegex
        /// </summary>
        private Regex UrlRegex { get; }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = (string)context.GetRouteValue("imagePath");

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
                                                                            .Select(x => x.Substring(0, x.Length - 1)) //remove "/"
                                                                            .ToArray();

            //unsafe url?
            if (Settings.Value.AllowUnsafeUrl && url_signature == "unsafe")
            {
                Logger.LogTrace("unsafe request");
            }
            else
            {
                //check signature
                string signature = new CryptoService(Settings.Value.Key).Encrypt(url_path);

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

            //generate key
            byte[] keyInBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(url_path));
            string key = keyInBytes.ToHexcode();

            IImageCache imageCache = context.RequestServices.GetService<IImageCache>();

            if (imageCache == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Image cache not found");

                return;
            }

            //try to get cached image
            ICachedImage cachedImage = await imageCache.ReadAsync(key);

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
                    Key = key,
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
                    using (Image image = Image.Load(originalImage.Data))
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
                            new ResizeFilter().Resize(width, height, ResizeMode.Max, filterContext);
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
                byte[] hash = SHA256.Create().ComputeHash(transformedImageData);

                imageMetadata.Hash = hash.ToHexcode();
                imageMetadata.FileLength = transformedImageData.Length;

                //disable cache?
                if (imageMetadata.NoImageCache == false)
                {
                    //save cached image
                    await imageCache.WriteAsync(key, imageMetadata, transformedImageData);
                }

                cachedImage = new CachedImage(imageMetadata, () => Task.FromResult<Stream>(new MemoryStream(transformedImageData)));

                interceptors.Foreach(x => x.OnCachedImageCreated(cachedImage));
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
                context.Response.ContentLength = cachedImage.Metadata.FileLength;
                context.Response.ContentType = cachedImage.Metadata.MimeType;

                interceptors.Foreach(x => x.OnResponseSending(context.Response, cachedImage));

                await stream.CopyToAsync(context.Response.Body);

                interceptors.Foreach(x => x.OnResponseCompleted(cachedImage));
            }

            Logger.LogTrace("Operation completed");
        }
    }
}
