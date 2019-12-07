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
using Microsoft.AspNetCore.Mvc;
using ImageWizard.AspNetCore;

namespace ImageWizard
{
    /// <summary>
    /// ImageUrlBuilder
    /// </summary>
    public class ImageUrlBuilder : IImageUrlBuilder, IImageFilters, IImageLoaderType
    {
        public IOptions<ImageWizardClientSettings> Settings { get; }
        public IWebHostEnvironment HostingEnvironment { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public IFileVersionProvider FileVersionProvider { get; }
        public IUrlHelper UrlHelper { get; internal set; }

        private CryptoService CryptoService { get; }
       
        private string LoaderSource { get; set; }

        private List<string> Filters { get; set; }

        private string LoaderType { get; set; }

        public ImageUrlBuilder(
            IOptions<ImageWizardClientSettings> settings,
            IWebHostEnvironment env, 
            IHttpContextAccessor httpContextAccessor,
            IFileVersionProvider fileVersionProvider)
        {
            if (settings.Value.UseUnsafeUrl == false)
            {
                CryptoService = new CryptoService(settings.Value.Key);
            }

            Settings = settings;
            HostingEnvironment = env;
            HttpContextAccessor = httpContextAccessor;
            FileVersionProvider = fileVersionProvider;

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
                signature = CryptoService.Encrypt(url.ToString());
            }

            url.Insert(0, "/");
            url.Insert(0, signature);
            url.Insert(0, "/");
            url.Insert(0, Settings.Value.BaseUrl);

            return url.ToString();
        }
    }
}
