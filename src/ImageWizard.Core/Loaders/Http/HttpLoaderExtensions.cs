// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard;

public static class HttpLoaderExtensions
{
    public static IImageWizardBuilder AddHttpLoader(this IImageWizardBuilder wizardConfiguration)
    {
        return AddHttpLoader(wizardConfiguration, options => { });
    }

    public static IImageWizardBuilder AddHttpLoader(this IImageWizardBuilder wizardConfiguration, Action<HttpLoaderOptions> setup)
    {
        wizardConfiguration.Services.Configure(setup);

        wizardConfiguration.Services.AddHttpClient<HttpLoader>();
        wizardConfiguration.LoaderManager.Register<HttpLoader>("fetch");

        return wizardConfiguration;
    }
}
