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
using ImageWizard.Services;
using ImageWizard.Settings;
using ImageWizard.Services.Types;
using ImageWizard.ImageFormats;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.SharedContract;
using System.Text;
using ImageWizard.ImageFormats.Base;
using Microsoft.AspNetCore.Http;
using ImageWizard.ImageLoaders;
using ImageWizard.ImageStorages;

namespace ImageWizard.Controllers
{
    //[Route("image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public ImageController(
            IOptions<ServiceSettings> settings,
            FilterManager filterManager,
            HttpLoader imageDownloader,
            CryptoService cryptoService,
            FileStorage fileService)
        {
            Settings = settings;
            FilterManager = filterManager;
            ImageLoader = imageDownloader;
            CryptoService = cryptoService;
            FileStorage = fileService;
        }

        private IOptions<ServiceSettings> Settings { get; }

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
        private IImageStorage FileStorage { get; }

        /// <summary>
        /// CryptoService
        /// </summary>
        private CryptoService CryptoService { get; }

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

        [HttpGet("{signatureRequest}/{*path}")]
        [ResponseCache(Duration = 60 * 60 * 24 * 7)]
        public async Task<IActionResult> Get(string signatureRequest, string path)
        {
            //add query to image url
            if(Request.QueryString.HasValue)
            {
                path += Request.QueryString.Value;
            }

            string signature = CryptoService.Encrypt(path);

            //check unsafe keyword or signature
            if ((Settings.Value.AllowUnsafeUrl && signatureRequest == "unsafe") == false 
                && 
                (signature == signatureRequest) == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            //convert signature to hex for the filestore
            byte[] buf = Encoding.UTF8.GetBytes(signature);
            string signatureHex = buf.Aggregate(string.Empty, (a, b) => a += b.ToString("x2"));

            //try to get cached image
            CachedImage cachedImage = await FileStorage.GetAsync(signatureHex);

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
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                string imageUrl = path.Substring(pos);
                
                //download image
                OriginalImage originalImage = await ImageLoader.GetAsync(imageUrl);
                
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
                                return StatusCode(StatusCodes.Status400BadRequest);
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

                cachedImage = await FileStorage.SaveAsync(signatureHex, originalImage, targetFormat, transformedImageData);
            }

            return File(cachedImage.Data, cachedImage.Metadata.MimeType);
        }

        [HttpGet("/random")]
        public IActionResult RandomKey()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            byte[] buf = new byte[64];
            random.NextBytes(buf);

            return Ok(Convert.ToBase64String(buf));
        }
    }
}
