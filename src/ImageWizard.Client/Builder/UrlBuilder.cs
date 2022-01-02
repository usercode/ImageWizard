using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client
{
    /// <summary>
    /// UrlBuilder
    /// </summary>
    public class UrlBuilder : ILoader
    {
        /// <summary>
        /// Settings
        /// </summary>
        public ImageWizardClientSettings Settings { get; }

        /// <summary>
        /// ServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        public UrlBuilder(
            IOptions<ImageWizardClientSettings> settings,
            IServiceProvider serviceProvider)
        {
            Settings = settings.Value;
            ServiceProvider = serviceProvider;
        }

        IFilter ILoader.LoadData(string loaderType, string loaderSource)
        {
            UrlBuilderContext context = new UrlBuilderContext(this);
            
            return context.LoadData(loaderType, loaderSource);
        }
    }
}
