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
            FilterManager filterManager
            )
        {
            Settings = settings;
            FilterManager = filterManager;
            Logger = logger;

            _next = next;

            CryptoService = new CryptoService(Settings.Value.Key);

            UrlRegex = new Regex($@"^{Settings.Value.BasePath.Value}/(?<signature>[a-z0-9-_]+)/(?<path>(?<filter>[a-z]+\([a-z0-9,.]*\)/)*(?<deliveryType>[a-z]+)/(?<imagesource>.*))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        private IOptions<ImageWizardSettings> Settings { get; }

        /// <summary>
        /// FilterManager
        /// </summary>
        private FilterManager FilterManager { get; }

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

            string url_signature = match.Groups["signature"].Value;
            string url_imagesource = match.Groups["imagesource"].Value;
            string url_path = match.Groups["path"].Value;
            string url_deliveryType = match.Groups["deliveryType"].Value;
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

            //check ETag from request
            if (Settings.Value.UseETag)
            {
                bool? isValid = context.Request.GetTypedHeaders().IfNoneMatch?.Any(x => x.Tag == $"\"{signature}\"");

                if (isValid == true)
                {
                    context.Response.StatusCode = StatusCodes.Status304NotModified;

                    return;
                }
            }

            IImageCache imageCache = context.RequestServices.GetService<IImageCache>();

            if(imageCache == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Image cache not found");

                return;
            }

            //try to get cached image
            CachedImage cachedImage = await imageCache.ReadAsync(signature);

            //no cached image found?
            if (cachedImage == null)
            {
                Logger.LogTrace("Create cached image");

                IImageLoader loader = context.RequestServices.GetServices<IImageLoader>().FirstOrDefault(x => x.DeliveryType == url_deliveryType);

                if(loader == null)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("image loader not found: " + url_deliveryType);

                    return;
                }

                Logger.LogTrace("Get original image: " + url_imagesource);

                //download image
                OriginalImage originalImage = await loader.GetAsync(url_imagesource);

                IImageFormat targetFormat = ImageFormatHelper.Parse(originalImage.MimeType);

                byte[] transformedImageData;

                //create metadata
                ImageMetadata imageMetadata = new ImageMetadata();
                imageMetadata.ImageSource = originalImage.Url;
                imageMetadata.Signature = signature;
                imageMetadata.MimeType = targetFormat.MimeType;
                imageMetadata.DPR = null;

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

                        //check DPR value from request
                        string dprString = context.Request.Headers["DPR"].FirstOrDefault();

                        if(dprString != null)
                        {
                            filterContext.DPR = double.Parse(dprString, CultureInfo.InvariantCulture);
                        }

                        //execute filters
                        foreach (string filter in url_filters)
                        {
                            bool filterFound = false;

                            foreach (FilterAction action in FilterManager.FilterActions)
                            {
                                if (action.TryExecute(filter, filterContext))
                                {
                                    Logger.LogTrace("Filter executed: " + filter);

                                    filterFound = true;
                                    break;
                                }
                            }

                            if (filterFound == false)
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

                        if(Settings.Value.ImageMaxHeight != null && height > Settings.Value.ImageMaxHeight)
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
                    }
                }
                
                cachedImage = new CachedImage();
                cachedImage.Metadata = imageMetadata;
                cachedImage.Data = transformedImageData;

                Logger.LogTrace("Save new cached image");

                await imageCache.WriteAsync(signature, cachedImage);
            }
            else
            {
                Logger.LogTrace("Cached image found");
            }

            //set cache control header
            if (Settings.Value.CacheControl.IsEnabled)
            {
                context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                {
                    Public = Settings.Value.CacheControl.Public,
                    MustRevalidate = Settings.Value.CacheControl.MustRevalidate,
                    MaxAge = Settings.Value.CacheControl.MaxAge
                };
            }

            //set ETag header
            if (Settings.Value.UseETag)
            {
                context.Response.GetTypedHeaders().ETag = new EntityTagHeaderValue($"\"{signature}\"");
            }

            //DPR
            if (cachedImage.Metadata.DPR != null)
            {
                context.Response.Headers.Add("Content-DPR", cachedImage.Metadata.DPR.ToString());
                context.Response.Headers.Add("Vary", "DPR");
            }

            context.Response.ContentLength = cachedImage.Data.Length;
            context.Response.ContentType = cachedImage.Metadata.MimeType;

            await context.Response.Body.WriteAsync(cachedImage.Data, 0, cachedImage.Data.Length);

            Logger.LogTrace("Operation completed");
        }
    }
}
