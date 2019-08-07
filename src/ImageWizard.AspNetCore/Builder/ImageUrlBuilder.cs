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
    public class ImageUrlBuilder : IImageUrlBuilder, IImageFilters, IImageDeliveryType
    {
        public CryptoService CryptoService { get; }
        public IOptions<ImageWizardClientSettings> Settings { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public IFileVersionProvider FileVersionProvider { get; }

        public string ImageUrl { get; set; }

        public List<string> Filters { get; set; }

        public string DeliveryType { get; set; }

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

            Filters = new List<string>();
        }

        public IImageFilters Image(string deliveryType, string url)
        {
            DeliveryType = deliveryType;
            ImageUrl = url;

            return this;
        }

        public IImageFilters Filter(string filter)
        {
            Filters.Add(filter);

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

            for (int i = 0; i < Filters.Count; i++)
            {
                url.Append(Filters[i]);
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
