﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

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

    /// <summary>
    /// Adds a pipeline.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mimeTypes"></param>
    void AddPipeline<T>(IEnumerable<string> mimeTypes) where T : class, IPipeline;
}
