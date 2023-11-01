// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;
using ImageWizard.Loaders;

namespace ImageWizard;

public static class FileLoaderExtensions
{
    public static IImageWizardBuilder AddFileLoader(this IImageWizardBuilder wizardConfiguration, Action<FileLoaderOptions>? options = null)
    {
        if (options != null)
        {
            wizardConfiguration.Services.Configure(options);
        }

        wizardConfiguration.Services.AddSingleton<FileLoader>();
        wizardConfiguration.LoaderManager.Register<FileLoader>("file");

        return wizardConfiguration;
    }
}
