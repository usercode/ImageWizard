using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ImageWizard.SharedContract.FilterTypes;
using System.Diagnostics;
using ImageWizard.SharedContract;
using ImageWizard.AspNetCore.Builder.Types;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ImageWizard.AspNetCore.Builder
{
    /// <summary>
    /// ImageUrlBuilder
    /// </summary>
    public class ImageUrlBuilder : IImageSelector, IImageFilters
    {
        private CryptoService CryptoService { get; }
        private IOptions<ImageWizardClientSettings> Settings { get; }
        private IHostingEnvironment HostingEnvironment { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IFileVersionProvider FileVersionProvider { get; }

        private string ImageUrl { get; set; }

        private List<string> _filter;

        private string DeliveryType { get; set; }

        public ImageUrlBuilder(IOptions<ImageWizardClientSettings> settings, 
            IHostingEnvironment env, 
            IHttpContextAccessor httpContextAccessor,
            IFileVersionProvider fileVersionProvider)
        {
            if (settings.Value.Key != null)
            {
                CryptoService = new CryptoService(settings.Value.Key);
            }

            Settings = settings;
            HostingEnvironment = env;
            HttpContextAccessor = httpContextAccessor;
            FileVersionProvider = fileVersionProvider;

            _filter = new List<string>();
        }

        /// <summary>
        /// Fetch file from absolute or relative url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IImageFilters Fetch(string url)
        {
            ImageUrl = url;
            DeliveryType = "fetch";

            return this;
        }

        /// <summary>
        /// Fetch file from wwwroot folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="addFingerprint"></param>
        /// <returns></returns>
        public IImageFilters FetchStaticFile(string path)
        {
            string newPath = FileVersionProvider.AddFileVersionToPath(HttpContextAccessor.HttpContext.Request.PathBase, path);

            ImageUrl = newPath;
            DeliveryType = "fetch";

            return this;
        }

        public IImageFilters File(string path)
        {
            ImageUrl = path;
            DeliveryType = "file";

            return this;
        }

        public IImageFilters Gravatar(string email)
        {
            var md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(email.Trim().ToLower()));

            ImageUrl = GetHashString(hash);
            DeliveryType = "gravatar";

            return this;
        }

        public static string GetHashString(byte[] hash)
        {
            StringBuilder sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public IImageFilters Youtube(string id)
        {
            ImageUrl = id;
            DeliveryType = "youtube";

            return this;
        }

        public IImageFilters DPR(double value)
        {
            _filter.Add($"dpr({value.ToString("0.0", CultureInfo.InvariantCulture)})");

            return this;
        }

        public IImageFilters Crop(int width, int heigth)
        {
            Crop(0, 0, width, heigth);

            return this;
        }

        public IImageFilters Crop(int x, int y, int width, int heigth)
        {
            _filter.Add($"crop({x},{y},{width},{heigth})");

            return this;
        }

        public IImageFilters Crop(double width, double heigth)
        {
            Crop(0, 0, width, heigth);

            return this;
        }

        public IImageFilters Crop(double x, double y, double width, double heigth)
        {
            _filter.Add($"crop({x.ToString("0.0", CultureInfo.InvariantCulture)},{y.ToString("0.0", CultureInfo.InvariantCulture)},{width.ToString("0.0", CultureInfo.InvariantCulture)},{heigth.ToString("0.0", CultureInfo.InvariantCulture)})");

            return this;
        }
        public IImageFilters Blur()
        {
            _filter.Add($"blur()");

            return this;
        }

        public IImageFilters Resize(int size)
        {
            _filter.Add($"resize({size})");

            return this;
        }

        public IImageFilters Resize(int width, int height)
        {
            _filter.Add($"resize({width},{height})");

            return this;
        }

        public IImageFilters Resize(int width, int height, ResizeMode mode)
        {
            _filter.Add($"resize({width},{height},{mode.ToString().ToLower()})");

            return this;
        }

        public IImageFilters Trim()
        {
            _filter.Add("trim()");

            return this;
        }

        public IImageFilters Grayscale()
        {
            _filter.Add($"grayscale()");

            return this;
        }

        public IImageFilters BlackWhite()
        {
            _filter.Add($"blackwhite()");

            return this;
        }

        public IImageFilters Rotate(RotateMode mode)
        {
            _filter.Add($"rotate({(int)mode})");

            return this;
        }

        public IImageFilters Flip(FlipMode flippingMode)
        {
            _filter.Add($"flip({flippingMode.ToString().ToLower()})");

            return this;
        }

        public IImageBuildUrl Jpg()
        {
            _filter.Add("jpg()");

            return this;
        }

        public IImageBuildUrl Jpg(int quality)
        {
            _filter.Add($"jpg({quality})");

            return this;
        }

        public IImageBuildUrl Png()
        {
            _filter.Add("png()");

            return this;
        }

        public IImageBuildUrl Gif()
        {
            _filter.Add("gif()");

            return this;
        }

        public IImageBuildUrl Bmp()
        {
            _filter.Add("bmp()");

            return this;
        }

        public HtmlString BuildUrl()
        {
            if(string.IsNullOrEmpty(ImageUrl))
            {
                throw new Exception("No image is selected.");
            }

            if (Settings.Value.Enabled == false)
            {
                return new HtmlString(ImageUrl);
            }

            StringBuilder url = new StringBuilder();

            for (int i = 0; i < _filter.Count; i++)
            {
                url.Append(_filter[i]);
                url.Append("/");
            }

            url.Append($"{DeliveryType}");

            if (ImageUrl.StartsWith("/") == false)
            {
                url.Append("/");
            }

            url.Append(ImageUrl);

            string signature = "unsafe";

            if (CryptoService != null)
            {
                signature = CryptoService.Encrypt(url.ToString());
            }

            url.Insert(0, "/");
            url.Insert(0, signature);
            url.Insert(0, "/");
            url.Insert(0, Settings.Value.BaseUrl);

            return new HtmlString(url.ToString());
        }
    }
}
