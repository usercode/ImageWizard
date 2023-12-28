// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.ImageSharp;

public static class PalceholderExtensions
{
    public static IImageWizardBuilder AddPlaceholder(this IImageWizardBuilder wizardConfiguration, Action<PlaceholderOptions>? options = null)
    {
        if (options != null)
        {
            wizardConfiguration.Services.Configure(options);
        }

        wizardConfiguration.Services.AddTransient<PlaceholderLoader>();
        wizardConfiguration.LoaderManager.Register<PlaceholderLoader>("placeholder");

        return wizardConfiguration;
    }
}
