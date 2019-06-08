using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ImageWizard.AspNetCore.Services;
using Microsoft.Extensions.Options;
using ImageWizard.SharedContract.FilterTypes;
using System.Diagnostics;

namespace ImageWizard.AspNetCore.Builder
{
    public class ImageUrlBuilder
    {
        private CryptoService CryptoService { get; }
        private ImageWizardSettings Settings { get; }

        public string ImageUrl { get; }

        private List<string> _filter;

        public ImageUrlBuilder(string imageUrl, ImageWizardSettings settings)
        {
            CryptoService = new CryptoService(settings);

            ImageUrl = imageUrl;
            Settings = settings;

            _filter = new List<string>();
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

        public ImageUrlBuilder Rotate(RotationMode mode)
        {
            _filter.Add($"rotate({(int)mode})");

            return this;
        }

        public ImageUrlBuilder Flp(FlippingMode flippingMode)
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
