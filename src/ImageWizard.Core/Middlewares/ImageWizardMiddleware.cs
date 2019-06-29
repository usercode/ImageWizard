using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.ImageLoaders;
using ImageWizard.ImageStorages;
using ImageWizard.Services.Types;
using ImageWizard.Settings;
using ImageWizard.SharedContract;
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
            IOptions<ImageWizardCoreSettings> settings,
            FilterManager filterManager,
            IImageLoader imageDownloader,
            IImageCache fileCache,
            CryptoService cryptoService
            )
        {
            Settings = settings;
            FilterManager = filterManager;
            ImageLoader = imageDownloader;
            FileCache = fileCache;
            CryptoService = cryptoService;

            _next = next;

            UrlRegex = new Regex($@"^{Settings.Value.BasePath.Value}/(?<signature>[A-Za-z0-9-_]{{27}}|unsafe)/(?<path>(?<filter>[a-z]+\(.*?\)/)*fetch/(?<imagesource>.*))$");
        }

        private IOptions<ImageWizardCoreSettings> Settings { get; }

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
        private IImageCache FileCache { get; }

        /// <summary>
        /// CryptoService
        /// </summary>
        private CryptoService CryptoService { get; }

        private Regex UrlRegex { get; }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value;

            if (context.Request.QueryString.HasValue)
            {
                path += context.Request.QueryString.Value;
            }

            Match match = UrlRegex.Match(path);

            if (match.Success == false)
            {
                await _next(context);

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
            CachedImage cachedImage = await FileCache.GetAsync(signature);

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
                        FilterContext filterContext = new FilterContext(image, targetFormat);

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

                        //target format is changed by user
                        targetFormat = filterContext.ImageFormat;

                        MemoryStream mem = new MemoryStream();

                        //generate image
                        targetFormat.SaveImage(image, mem);

                        transformedImageData = mem.ToArray();
                    }
                }

                cachedImage = await FileCache.SaveAsync(signature, originalImage, targetFormat, transformedImageData);
            }

            //send cached and transformed image
            if (Settings.Value.ResponseCacheTime != null)
            {
                context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = Settings.Value.ResponseCacheTime
                };
            }

            if (Settings.Value.UseETag)
            {
                context.Response.GetTypedHeaders().ETag = new EntityTagHeaderValue($"\"{signature}\"");
            }

            context.Response.ContentLength = cachedImage.Data.Length;
            context.Response.ContentType = cachedImage.Metadata.MimeType;
            context.Response.Body.Write(cachedImage.Data, 0, cachedImage.Data.Length);
        }
    }
}
