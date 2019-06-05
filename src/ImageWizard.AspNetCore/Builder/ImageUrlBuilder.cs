using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ImageWizard.AspNetCore.Services;
using Microsoft.Extensions.Options;

namespace ImageWizard.AspNetCore.Builder
{
    public class ImageUrlBuilder
    {
        private CryptoService CryptoService { get; }
        private ImageWizardSettings Settings { get; }

        public string ImageUrl { get; set; }

        private List<string> _filter;

        public ImageUrlBuilder(CryptoService cryptoService, IOptions<ImageWizardSettings> settings)
        {
            CryptoService = cryptoService;

            Settings = settings.Value;

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

        public HtmlString BuildUrl()
        {
            StringBuilder url = new StringBuilder();

            for(int i = 0; i< _filter.Count; i++)
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
