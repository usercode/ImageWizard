// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ImageWizard.Loaders;

namespace ImageWizard;

public static class FileLoaderExtensions
{
    public static IImageWizardBuilder AddFileLoader(this IImageWizardBuilder wizardConfiguration)
    {
        return AddFileLoader(wizardConfiguration, setup => { });
    }

    public static IImageWizardBuilder AddFileLoader(this IImageWizardBuilder wizardConfiguration, Action<FileLoaderOptions> setup)
    {
        wizardConfiguration.Services.Configure(setup);
        wizardConfiguration.Services.AddSingleton<FileLoader>();
        wizardConfiguration.LoaderManager.Register<FileLoader>("file");

        return wizardConfiguration;
    }
}
