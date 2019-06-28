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

namespace ImageWizard.AspNetCore.Builder
{
    public class ImageUrlBuilder
    {
        private CryptoService CryptoService { get; }
        private ImageWizardSettings Settings { get; }

        private string ImageUrl { get; set; }

        private List<string> _filter;

        public ImageUrlBuilder(ImageWizardSettings settings)
        {
            CryptoService = new CryptoService(settings.Key);

            Settings = settings;

            _filter = new List<string>();
        }

        public ImageUrlBuilder Image(string url)
        {
            ImageUrl = url;

            return this;
        }

        public ImageUrlBuilder Crop(int width, int heigth)
        {
            Crop(0, 0, width, heigth);

            return this;
        }

        public ImageUrlBuilder Crop(int x, int y, int width, int heigth)
        {
            _filter.Add($"crop({x},{y},{width},{heigth})");

            return this;
        }

        public ImageUrlBuilder Crop(double width, double heigth)
        {
            Crop(0, 0, width, heigth);

            return this;
        }

        public ImageUrlBuilder Crop(double x, double y, double width, double heigth)
        {
            _filter.Add($"crop({x:0.0},{y:0.0},{width:0.0},{heigth:0.0})");

            return this;
        }

        public ImageUrlBuilder Resize(int size)
        {
            _filter.Add($"resize({size})");

            return this;
        }

        public ImageUrlBuilder Resize(int width, int height)
        {
            _filter.Add($"resize({width},{height})");

            return this;
        }

        public ImageUrlBuilder Resize(int width, int height, ResizeMode mode)
        {
            _filter.Add($"resize({width},{height},{mode.ToString().ToLower()})");

            return this;
        }

        public ImageUrlBuilder Trim()
        {
            _filter.Add("trim()");

            return this;
        }

        public ImageUrlBuilder Grayscale()
        {
            _filter.Add($"grayscale()");

            return this;
        }

        public ImageUrlBuilder BlackWhite()
        {
            _filter.Add($"blackwhite()");

            return this;
        }

        public ImageUrlBuilder Rotate(RotateMode mode)
        {
            _filter.Add($"rotate({(int)mode})");

            return this;
        }

        public ImageUrlBuilder Flp(FlipMode flippingMode)
        {
            _filter.Add($"flip({flippingMode.ToString().ToLower()})");

            return this;
        }

        public ImageUrlBuilder Jpg()
        {
            _filter.Add("jpg()");

            return this;
        }

        public ImageUrlBuilder Jpg(int quality)
        {
            _filter.Add($"jpg({quality})");

            return this;
        }

        public ImageUrlBuilder Png()
        {
            _filter.Add("png()");

            return this;
        }

        public ImageUrlBuilder Gif()
        {
            _filter.Add("gif()");

            return this;
        }

        public ImageUrlBuilder Bmp()
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

            if (Settings.Enabled == false)
            {
                return new HtmlString(ImageUrl);
            }

            StringBuilder url = new StringBuilder();

            for (int i = 0; i < _filter.Count; i++)
            {
                url.Append(_filter[i]);
                url.Append("/");
            }

            url.Append("fetch/");
            url.Append(ImageUrl);

            string secret = CryptoService.Encrypt(url.ToString());

            url.Insert(0, "/");
            url.Insert(0, secret);
            url.Insert(0, "/");
            url.Insert(0, Settings.BaseUrl);

            return new HtmlString(url.ToString());
        }
    }
}
