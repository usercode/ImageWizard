// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.DependencyInjection;

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
