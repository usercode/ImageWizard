// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard;

public static class OpenGraphExtensions
{
    public static IImageWizardBuilder AddOpenGraphLoader(this IImageWizardBuilder wizardConfiguration, Action<OpenGraphOptions>? options = null)
    {
        if (options != null)
        {
            wizardConfiguration.Services.Configure(options);
        }
        wizardConfiguration.Services.AddHttpClient<OpenGraphLoader>();
        wizardConfiguration.LoaderManager.Register<OpenGraphLoader>("opengraph");

        return wizardConfiguration;
    }
}
