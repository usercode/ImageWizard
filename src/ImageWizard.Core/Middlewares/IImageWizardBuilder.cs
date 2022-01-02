﻿using ImageWizard.Processing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
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
        /// LoaderManager
        /// </summary>
        TypeManager LoaderManager { get; }

        /// <summary>
        /// PipelineManager
        /// </summary>
        TypeManager PipelineManager { get; }

        void AddPipeline<T>(string[] mimeTypes) where T : class, IProcessingPipeline;
    }
}
