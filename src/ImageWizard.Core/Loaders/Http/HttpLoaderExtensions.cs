// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

public static class HttpLoaderExtensions
{
    public static IImageWizardBuilder AddHttpLoader(this IImageWizardBuilder wizardConfiguration, Action<HttpLoaderOptions>? options = null)
    {
        if (options != null)
        {
            wizardConfiguration.Services.Configure(options);
        }

        wizardConfiguration.Services.AddHttpClient<HttpLoader>();
        wizardConfiguration.LoaderManager.Register<HttpLoader>("fetch");

        return wizardConfiguration;
    }
}
