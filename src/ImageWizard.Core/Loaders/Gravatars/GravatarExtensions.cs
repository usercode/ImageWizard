// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard;

public static class GravatarExtensions
{
    public static IImageWizardBuilder AddGravatarLoader(this IImageWizardBuilder wizardConfiguration)
    {
        return AddGravatarLoader(wizardConfiguration, x => { });
    }

    public static IImageWizardBuilder AddGravatarLoader(this IImageWizardBuilder wizardConfiguration, Action<GravatarOptions> options)
    {
        wizardConfiguration.Services.Configure(options);
        wizardConfiguration.Services.AddHttpClient<GravatarLoader>();
        wizardConfiguration.LoaderManager.Register<GravatarLoader>("gravatar");

        return wizardConfiguration;
    }
}
