using ImageWizard.Core.ImageFilters.Base;
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
    public delegate void PipelineAction<T>(T pipeline);

    /// <summary>
    /// ServiceConfiguration
    /// </summary>
    public class ImageWizardBuilder : IImageWizardBuilder
    {
        public ImageWizardBuilder(IServiceCollection services)
        {
            Services = services;

            ImageLoaderManager = new TypeManager();
            PipelineManager = new TypeManager();
        }

        /// <summary>
        /// Service
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// ImageLoaderManager
        /// </summary>
        public TypeManager ImageLoaderManager { get; }

        /// <summary>
        /// PipelineManager
        /// </summary>
        public TypeManager PipelineManager { get; }

        public IEnumerable<string> GetAllMimeTypes()
        {
            return PipelineManager.GetAllKeys();
        }

        public void AddPipeline<T>(string[] mimeTypes) 
            where T : class, IProcessingPipeline
        {
            Services.AddSingleton<T>();

            foreach(string mimeType in mimeTypes)
            {
                PipelineManager.Register<T>(mimeType);
            }

            Services.AddSingleton(new PipelineAction<T>(x => x.UsedMimeTypes = mimeTypes));
        }

        public Type GetPipeline(string key)
        {
            Type type = PipelineManager.Get(key);

            return type;
        }
    }
}
