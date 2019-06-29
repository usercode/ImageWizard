using ImageWizard.Core.Middlewares;
using ImageWizard.ImageStorages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageCaches
{
    public static class FileCacheExtensions
    {
        public static IImageWizardBuilder AddFileCache(this IImageWizardBuilder wizardConfiguration)
        {
            return AddFileCache(wizardConfiguration, options => { });
        }

        public static IImageWizardBuilder AddFileCache(this IImageWizardBuilder wizardConfiguration, Action<FileCacheSettings> fileCacheSettingsSetup)
        {
            wizardConfiguration.Services.Configure(fileCacheSettingsSetup);

            wizardConfiguration.Services.AddSingleton<IImageCache, FileCache>();

            return wizardConfiguration;
        }
    }
}
