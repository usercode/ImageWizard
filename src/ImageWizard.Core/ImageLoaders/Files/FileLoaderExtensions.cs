using ImageWizard.Core.Middlewares;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ImageWizard.ImageLoaders;
using ImageWizard.Core.ImageLoaders.Files;

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
            wizardConfiguration.Services.AddSingleton<FileLoader>();
            wizardConfiguration.ImageLoaderManager.Register<FileLoader>("file");

            return wizardConfiguration;
        }
    }
}
