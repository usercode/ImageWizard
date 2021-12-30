using ImageWizard.Client.Builder.Types;
using ImageWizard.Core;
using ImageWizard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    /// <summary>
    /// UrlBuilderContext
    /// </summary>
    class UrlBuilderContext : ILoader, IFilter, IBuildUrl
    {
        public UrlBuilderContext(UrlBuilder imageUrlBuilder)
        {
            ImageUrlBuilder = imageUrlBuilder;
            Filters = new List<string>();
        }

        public UrlBuilder ImageUrlBuilder { get; }
        private List<string> Filters { get; }
        private string LoaderSource { get; set; }
        private string LoaderType { get; set; }

        public ImageWizardClientSettings Settings => ImageUrlBuilder.Settings;
        public IServiceProvider ServiceProvider => ImageUrlBuilder.ServiceProvider;

        public IFilter LoadData(string loaderType, string loaderSource)
        {
            LoaderType = loaderType;
            LoaderSource = loaderSource;

            return this;
        }

        public IFilter Filter(string filter)
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

            ImageWizardUrl url;

            if (ImageUrlBuilder.Settings.UseUnsafeUrl)
            {
                url = ImageWizardUrl.CreateUnsafe(LoaderType, LoaderSource, Filters);
            }
            else
            {
                url = ImageWizardUrl.Create(ImageUrlBuilder.Settings.Key, LoaderType, LoaderSource, Filters);
            }

            return $"{ImageUrlBuilder.Settings.BaseUrl.TrimEnd('/')}/{url.Signature}/{url.Path}";
        }

       
    }
}
