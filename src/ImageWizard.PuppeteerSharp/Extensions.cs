// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.PuppeteerSharp;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ImageWizard;

public static class Extensions
{
    public static IImageWizardBuilder AddPuppeteerLoader(this IImageWizardBuilder wizardConfiguration)
    {
        return AddPuppeteerLoader(wizardConfiguration, x => { });
    }

    public static IImageWizardBuilder AddPuppeteerLoader(this IImageWizardBuilder wizardConfiguration, Action<PuppeteerOptions> setup)
    {
        wizardConfiguration.Services.Configure(setup);

        wizardConfiguration.Services.AddTransient<ScreenshotLoader>();
        wizardConfiguration.LoaderManager.Register<ScreenshotLoader>("screenshot");

        return wizardConfiguration;
    }
}
