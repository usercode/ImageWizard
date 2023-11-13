// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.Core.Processing.Builder;

/// <summary>
/// PipelineBuilder
/// </summary>
public class PipelineBuilder : IPipelineBuilder
{
    public PipelineBuilder(IServiceCollection services)
    {
        Services = services;
    }

    /// <summary>
    /// MimeTypes
    /// </summary>
    public IList<string> MimeTypes { get; private set; } = new List<string>();

    /// <summary>
    /// ServiceProvider
    /// </summary>
    public IServiceCollection Services { get; private set; }
}
