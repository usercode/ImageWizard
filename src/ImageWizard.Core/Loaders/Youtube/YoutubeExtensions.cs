// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard;

public static class YouTubeExtensions
{
    public static IImageWizardBuilder AddYoutubeLoader(this IImageWizardBuilder wizardConfiguration)
    {
        return AddYoutubeLoader(wizardConfiguration, x => { });
    }

    public static IImageWizardBuilder AddYoutubeLoader(this IImageWizardBuilder wizardConfiguration, Action<YouTubeOptions> options)
    {
        wizardConfiguration.Services.Configure(options);
        wizardConfiguration.Services.AddHttpClient<YouTubeLoader>();
        wizardConfiguration.LoaderManager.Register<YouTubeLoader>("youtube");

        return wizardConfiguration;
    }
}
