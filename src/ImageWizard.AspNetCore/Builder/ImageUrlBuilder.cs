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

namespace ImageWizard.AspNetCore.Builder
{
    public class ImageUrlBuilder : IImageSelector, IImageFilters
    {
        private CryptoService CryptoService { get; }
        private IOptions<ImageWizardSettings> Settings { get; }

        private string ImageUrl { get; set; }

        private List<string> _filter;

        private string DeliveryType { get; set; }

        public ImageUrlBuilder(IOptions<ImageWizardSettings> settings)
        {
            CryptoService = new CryptoService(settings.Value.Key);

            Settings = settings;

            _filter = new List<string>();
        }

        public IImageFilters Fetch(string url)
        {
            ImageUrl = url;
            DeliveryType = "fetch";

            return this;
        }

        public IImageFilters Upload(string path)
        {
            ImageUrl = path;
            DeliveryType = "upload";

            return this;
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

            url.Append($"{DeliveryType}/");
            url.Append(ImageUrl);

            string secret = CryptoService.Encrypt(url.ToString());

            url.Insert(0, "/");
            url.Insert(0, secret);
            url.Insert(0, "/");
            url.Insert(0, Settings.Value.BaseUrl);

            return new HtmlString(url.ToString());
        }
    }
}
