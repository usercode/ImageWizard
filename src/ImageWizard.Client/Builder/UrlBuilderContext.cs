﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.Client;

/// <summary>
/// UrlBuilderContext
/// </summary>
class UrlBuilderContext : ILoader, IFilter, IBuildUrl
{
    public UrlBuilderContext(UrlBuilder imageUrlBuilder)
    {
        ImageUrlBuilder = imageUrlBuilder;
    }

    public UrlBuilder ImageUrlBuilder { get; }
    private List<FilterSegment> Filters { get; } = new List<FilterSegment>();
    private string? LoaderSource { get; set; }
    private string? LoaderType { get; set; }

    public ImageWizardClientSettings Settings => ImageUrlBuilder.Settings;
    public IServiceProvider ServiceProvider => ImageUrlBuilder.ServiceProvider;

    public IFilter LoadData(string loaderType, string loaderSource)
    {
        LoaderType = loaderType;
        LoaderSource = loaderSource;

        return this;
    }

    public IFilter Filter(FilterSegment filter)
    {
        Filters.Add(filter);

        return this;
    }

    public string BuildUrl()
    {
        ArgumentException.ThrowIfNullOrEmpty(LoaderType);
        ArgumentException.ThrowIfNullOrEmpty(LoaderSource);

        if (ImageUrlBuilder.Settings.Enabled == false)
        {
            return LoaderSource;
        }

        string signature = ImageWizardDefaults.Unsafe;

        ImageWizardUrl url = new ImageWizardUrl(LoaderType, LoaderSource, Filters.ToArray());

        if (ImageUrlBuilder.Settings.UseUnsafeUrl == false)
        {
            IUrlSignature signatureService = ServiceProvider.GetRequiredService<IUrlSignature>();

            signature = signatureService.Encrypt(ImageUrlBuilder.Settings.Key, new ImageWizardRequest(url, new HostString(ImageUrlBuilder.Settings.Host)));
        }

        return $"{ImageUrlBuilder.Settings.BaseUrl.TrimEnd('/')}/{signature}/{url.Path}";
    }
}
