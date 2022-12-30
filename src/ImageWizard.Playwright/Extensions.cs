// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Playwright;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ImageWizard;

public static class Extensions
{
    public static IImageWizardBuilder AddPlaywrightLoader(this IImageWizardBuilder wizardConfiguration)
    {
        return AddPlaywrightLoader(wizardConfiguration, x => { });
    }

    public static IImageWizardBuilder AddPlaywrightLoader(this IImageWizardBuilder wizardConfiguration, Action<PlaywrightOptions> setup)
    {
        wizardConfiguration.Services.Configure(setup);

        wizardConfiguration.Services.AddTransient<ScreenshotLoader>();
        wizardConfiguration.LoaderManager.Register<ScreenshotLoader>("screenshot");

        return wizardConfiguration;
    }
}
