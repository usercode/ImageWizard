using ImageWizard.Core.ImageLoaders;
using ImageWizard.Filters;
using ImageWizard.ImageLoaders;
using ImageWizard.ImageStorages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.Middlewares
{
    /// <summary>
    /// IImageWizardBuilder
    /// </summary>
    public interface IImageWizardBuilder
    {
        /// <summary>
        /// Services
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// FilterManager
        /// </summary>
        FilterManager FilterManager { get; }

        /// <summary>
        /// ImageLoaderManager
        /// </summary>
        ImageLoaderManager ImageLoaderManager { get; }

    }
}
