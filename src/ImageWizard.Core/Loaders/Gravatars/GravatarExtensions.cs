// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

public static class GravatarExtensions
{
    public static IImageWizardBuilder AddGravatarLoader(this IImageWizardBuilder wizardConfiguration, Action<GravatarOptions>? options = null)
    {
        if (options != null)
        {
            wizardConfiguration.Services.Configure(options);
        }

        wizardConfiguration.Services.AddHttpClient<GravatarLoader>();
        wizardConfiguration.LoaderManager.Register<GravatarLoader>("gravatar");

        return wizardConfiguration;
    }
}
