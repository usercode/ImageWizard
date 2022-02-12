using ImageWizard.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Client
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

            IUrlSignature signatureService = ServiceProvider.GetRequiredService<IUrlSignature>();

            string signature = ImageWizardDefaults.Unsafe;

            ImageWizardUrl url = new ImageWizardUrl(LoaderType, LoaderSource, Filters.ToArray());

            if (ImageUrlBuilder.Settings.UseUnsafeUrl == false)
            {
                signature = signatureService.Encrypt(ImageUrlBuilder.Settings.KeyInBytes, new ImageWizardRequest(url, new HostString(ImageUrlBuilder.Settings.Host)));
            }

            return $"{ImageUrlBuilder.Settings.BaseUrl.TrimEnd('/')}/{signature}/{url.Path}";
        }
    }
}
