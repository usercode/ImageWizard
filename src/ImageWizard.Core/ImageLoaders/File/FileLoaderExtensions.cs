using ImageWizard.Core.ImageLoaders.File;
using ImageWizard.Core.Middlewares;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ImageWizard.ImageLoaders;

namespace ImageWizard
{
    public static class FileLoaderExtensions
    {
        public static IImageWizardBuilder AddFileLoader(this IImageWizardBuilder wizardConfiguration)
        {
            return AddFileLoader(wizardConfiguration, setup => { });
        }

        public static IImageWizardBuilder AddFileLoader(this IImageWizardBuilder wizardConfiguration, Action<FileLoaderSettings> setup)
        {
            wizardConfiguration.Services.Configure(setup);
            wizardConfiguration.Services.AddSingleton<IImageLoader, FileLoader>();

            return wizardConfiguration;
        }
    }
}
