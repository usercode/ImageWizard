using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Settings;
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
        /// ImageLoaderManager
        /// </summary>
        TypeManager ImageLoaderManager { get; }

        /// <summary>
        /// PipelineManager
        /// </summary>
        TypeManager PipelineManager { get; }

        void AddPipeline<T>(string[] mimeTypes) where T : class, IProcessingPipeline;
    }
}
