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
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            FilterManager filterManager,
            IImageLoader imageLoader,
            IImageCache imageCache
            )
        {
            Settings = settings;
            FilterManager = filterManager;
            ImageLoader = imageLoader;
            ImageCache = imageCache;

            _next = next;

            CryptoService = new CryptoService(Settings.Value.Key);

            UrlRegex = new Regex($@"^{Settings.Value.BasePath.Value}/(?<signature>[a-z0-9-_]+)/(?<path>(?<filter>[a-z]+\([a-z0-9,]*\)/)*fetch/(?<imagesource>.*))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        private IOptions<ImageWizardSettings> Settings { get; }

        /// <summary>
        /// FilterManager
        /// </summary>
        private FilterManager FilterManager { get; }

        /// <summary>
        /// ImageDownloader
        /// </summary>
        private IImageLoader ImageLoader { get; }

        /// <summary>
        /// FileStorage
        /// </summary>
        private IImageCache ImageCache { get; }

        /// <summary>
        /// CryptoService
        /// </summary>
        private CryptoService CryptoService { get; }

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

            //try to get cached image
            CachedImage cachedImage = await ImageCache.ReadAsync(signature);

            //no cached image found?
            if (cachedImage == null)
            {
                //download image
                OriginalImage originalImage = await ImageLoader.GetAsync(url_imagesource);

                IImageFormat targetFormat = ImageFormatHelper.Parse(originalImage.MimeType);

                byte[] transformedImageData;

                //skip svg
                if (targetFormat is SvgFormat)
                {
                    transformedImageData = originalImage.Data;
                }
                else
                {
                    //load image
                    using (Image<Rgba32> image = Image.Load(originalImage.Data))
                    {
                        FilterContext filterContext = new FilterContext(Settings.Value, image, targetFormat);

                        //execute filters
                        foreach (string filter in url_filters)
                        {
                            bool filterFound = false;

                            foreach (FilterAction action in FilterManager.FilterActions)
                            {
                                if (action.TryExecute(filter, filterContext))
                                {
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

                        //target format is changed by user
                        targetFormat = filterContext.ImageFormat;

                        MemoryStream mem = new MemoryStream();

                        //generate image
                        targetFormat.SaveImage(image, mem);

                        transformedImageData = mem.ToArray();
                    }
                }

                //create metadata
                ImageMetadata imageMetadata = new ImageMetadata();
                imageMetadata.MimeType = targetFormat.MimeType;
                imageMetadata.Url = originalImage.Url;
                imageMetadata.Signature = signature;

                cachedImage = new CachedImage();
                cachedImage.Metadata = imageMetadata;
                cachedImage.Data = transformedImageData;

                await ImageCache.WriteAsync(signature, cachedImage);
            }

            //send cached and transformed image
            if (Settings.Value.ResponseCacheTime != null)
            {
                context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                {
                    Public = true,
                    MustRevalidate = true,
                    MaxAge = Settings.Value.ResponseCacheTime                    
                };
            }

            if (Settings.Value.UseETag)
            {
                context.Response.GetTypedHeaders().ETag = new EntityTagHeaderValue($"\"{signature}\"");
            }

            context.Response.ContentLength = cachedImage.Data.Length;
            context.Response.ContentType = cachedImage.Metadata.MimeType;

            await context.Response.Body.WriteAsync(cachedImage.Data, 0, cachedImage.Data.Length);
        }
    }
}
