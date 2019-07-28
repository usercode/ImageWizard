using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.ImageLoaders;
using ImageWizard.ImageStorages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ImageWizard.Core.Settings
{
    /// <summary>
    /// ServiceConfiguration
    /// </summary>
    public class ImageWizardBuilder : IImageWizardBuilder
    {
        public ImageWizardBuilder(IServiceCollection services)
        {
            Services = services;

            FilterManager = new FilterManager();
            ImageLoaderManager = new ImageLoaderManager();

            Services.AddSingleton(FilterManager);
            Services.AddSingleton(ImageLoaderManager);
        }

        /// <summary>
        /// Service
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// FilterManager
        /// </summary>
        public FilterManager FilterManager { get; }

        /// <summary>
        /// ImageLoaderManager
        /// </summary>
        public ImageLoaderManager ImageLoaderManager { get; }

        
    }
}
