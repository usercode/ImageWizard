// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Options;
using System;

namespace ImageWizard.Client;

/// <summary>
/// UrlBuilder
/// </summary>
class UrlBuilder : IImageWizardUrlBuilder, ILoader
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
