using ImageWizard.Client.Builder.Types;
using ImageWizard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    /// <summary>
    /// ImageUrlBuilderContext
    /// </summary>
    internal class ImageUrlBuilderContext : IImageFilters, IImageBuildUrl
    {
        public ImageUrlBuilderContext(ImageUrlBuilder imageUrlBuilder)
        {
            ImageUrlBuilder = imageUrlBuilder;
            Filters = new List<string>();
        }

        public ImageUrlBuilder ImageUrlBuilder { get; }
        private List<string> Filters { get; }
        private string LoaderSource { get; set; }
        private string LoaderType { get; set; }

        public ImageWizardClientSettings Settings => ImageUrlBuilder.Settings;
        public IServiceProvider ServiceProvider => ImageUrlBuilder.ServiceProvider;

        public IImageFilters Loader(string loaderType, string loaderSource)
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
            if (string.IsNullOrEmpty(LoaderSource))
            {
                throw new Exception("No image is selected.");
            }

            if (ImageUrlBuilder.Settings.Enabled == false)
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

            if (ImageUrlBuilder.Settings.UseUnsafeUrl == true)
            {
                signature = "unsafe";
            }
            else
            {
                signature = new CryptoService(ImageUrlBuilder.Settings.Key).Encrypt(url.ToString());
            }

            url.Insert(0, "/");
            url.Insert(0, signature);
            if (ImageUrlBuilder.Settings.BaseUrl.EndsWith("/") == false)
            {
                url.Insert(0, "/");
            }
            url.Insert(0, ImageUrlBuilder.Settings.BaseUrl);

            return url.ToString();
        }
    }
}
