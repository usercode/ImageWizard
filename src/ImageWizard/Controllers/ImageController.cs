using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ImageWizard.Filters;
using ImageWizard.Helpers;
using ImageWizard.Services;
using ImageWizard.Settings;
using System.Net;
using ImageWizard.Services.Types;
using ImageWizard.ImageFormats;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.SharedContract;

namespace ImageWizard.Controllers
{
    //[Route("image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public ImageController(
            FilterManager filterManager,
            ImageService imageDownloader,
            CryptoService cryptoService,
            FileService fileService)
        {
            FilterManager = filterManager;
            ImageDownloader = imageDownloader;
            CryptoService = cryptoService;
            FileService = fileService;
        }

        /// <summary>
        /// FilterManager
        /// </summary>
        private FilterManager FilterManager { get; }

        /// <summary>
        /// ImageDownloader
        /// </summary>
        private ImageService ImageDownloader { get; }

        /// <summary>
        /// CryptoService
        /// </summary>
        private CryptoService CryptoService { get; }

        /// <summary>
        /// FileService
        /// </summary>
        private FileService FileService { get; }

        [HttpGet("/")]
        public IActionResult Home()
        {
            return Ok("ImageWizard is started.");
        }

        [HttpGet("favicon.ico")]
        public IActionResult Favicon()
        {
            return NotFound();
        }

        [HttpGet("{secretKeyByRequest}/{*path}")]
        [ResponseCache(Duration = 60 * 60 * 24 * 7)]
        public async Task<IActionResult> Get(string secretKeyByRequest, string path)
        {
            string secretKey = CryptoService.Encrypt(path);

#if DEBUG == false
            //check secret
            if(secretKey != secretKeyByRequest)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
#endif

            //try to get cached image
            CachedImage cachedImage = await FileService.GetImageAsync(secretKey);

            //no cached image found?
            if (cachedImage == null)
            {
                //find url
                int pos = path.IndexOf("https://");

                if(pos == -1)
                {
                    pos = path.IndexOf("http://");
                }

                if (pos == -1)
                {
                    throw new Exception();
                }

                string imageUrl = path.Substring(pos);
                byte[] transformedImageData;

                //download image
                OriginalImage originalImage = await ImageDownloader.DownloadAsync(imageUrl);
                
                IImageFormat targetFormat = ImageFormatHelper.Parse(originalImage.MimeType);

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
                        FilterContext filterContext = new FilterContext(image);
                        filterContext.ImageFormat = targetFormat;

                        string filters = path.Substring(0, pos);
                        string[] segments = filters.Split("/", StringSplitOptions.RemoveEmptyEntries);

                        //execute filters
                        foreach (string segment in segments)
                        {
                            bool filterFound = false;

                            foreach (FilterAction action in FilterManager.FilterActions)
                            {
                                if (action.TryExecute(segment, filterContext))
                                {
                                    filterFound = true;
                                    break;
                                }
                            }

                            if (filterFound == false)
                            {
                                return StatusCode((int)HttpStatusCode.BadRequest);
                            }
                        }

                        MemoryStream mem = new MemoryStream();

                        //generate image
                        filterContext.ImageFormat.SaveImage(image, mem);

                        transformedImageData = mem.ToArray();

                        //target format is changed by user
                        targetFormat = filterContext.ImageFormat;
                    }
                }

                cachedImage = await FileService.SaveImageAsync(secretKey, originalImage, targetFormat, transformedImageData);
            }

            return File(cachedImage.Data, cachedImage.Metadata.MimeType);
        }

        [HttpGet("/random")]
        public IActionResult RandomKey()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            byte[] buf = new byte[64];
            random.NextBytes(buf);

            return Ok(Base64Url.ToBase64Url(buf));
        }
    }
}
