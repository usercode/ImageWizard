using ImageWizard.Client.Builder.Types;
using ImageWizard.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// ImageUrlBuilder
    /// </summary>
    public class ImageUrlBuilder : IImageUrlBuilder, IImageFilters, IImageLoaderType
    {
        public IOptions<ImageWizardClientSettings> Settings { get; }
        public IWebHostEnvironment HostEnvironment { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public IUrlHelper UrlHelper { get; internal set; }
       
        private string LoaderSource { get; set; }

        private List<string> Filters { get; set; }

        private string LoaderType { get; set; }

        public ImageUrlBuilder(
            IOptions<ImageWizardClientSettings> settings,
            IWebHostEnvironment env, 
            IHttpContextAccessor httpContextAccessor)
        {
            Settings = settings;
            HostEnvironment = env;
            HttpContextAccessor = httpContextAccessor;

            Filters = new List<string>();
        }

        public IImageFilters Image(string loaderType, string loaderSource)
        {
            LoaderType = loaderType;
            LoaderSource = loaderSource;

            return this;
        }

        public IImageFilters Filter(string filter)
        {
            Filters.Add(filter);

            return this;
        }

        public string BuildUrl()
        {
            if(string.IsNullOrEmpty(LoaderSource))
            {
                throw new Exception("No image is selected.");
            }

            if (Settings.Value.Enabled == false)
            {
                return LoaderSource;
            }

            StringBuilder url = new StringBuilder();

            for (int i = 0; i < Filters.Count; i++)
            {
                url.Append(Filters[i]);
                url.Append("/");
            }

            url.Append(LoaderType);

            if (LoaderSource.StartsWith("/") == false)
            {
                url.Append("/");
            }

            url.Append(LoaderSource);

            string signature;

            if (Settings.Value.UseUnsafeUrl == true)
            {
                signature = "unsafe";
            }
            else
            {
                signature = new CryptoService(Settings.Value.Key).Encrypt(url.ToString());
            }

            url.Insert(0, "/");
            url.Insert(0, signature);
            if (Settings.Value.BaseUrl.EndsWith("/") == false)
            {
                url.Insert(0, "/");
            }
            url.Insert(0, Settings.Value.BaseUrl);

            return url.ToString();
        }
    }
}
