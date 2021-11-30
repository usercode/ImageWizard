using ImageWizard.Client.Builder.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// ImageUrlBuilder
    /// </summary>
    public class ImageUrlBuilder : IImageLoader
    {
        public ImageWizardClientSettings Settings { get; }

        public IServiceProvider ServiceProvider { get; }

        public ImageUrlBuilder(
            IOptions<ImageWizardClientSettings> settings,
            IServiceProvider serviceProvider)
        {
            Settings = settings.Value;
            ServiceProvider = serviceProvider;
        }

        IImageFilters IImageLoader.Image(string loaderType, string loaderSource)
        {
            ImageUrlBuilderContext context = new ImageUrlBuilderContext(this);
            
            return context.Loader(loaderType, loaderSource);
        }
    }
}
