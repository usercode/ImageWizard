using ImageWizard.Client.Builder.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// UrlBuilder
    /// </summary>
    public class UrlBuilder : ILoader
    {
        public ImageWizardClientSettings Settings { get; }

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
