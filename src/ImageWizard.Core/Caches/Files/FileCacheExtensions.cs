using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ImageWizard.Caches;

namespace ImageWizard
{
    public static class FileCacheExtensions
    {
        public static IImageWizardBuilder SetDistributedCache(this IImageWizardBuilder wizardBuilder)
        {
            wizardBuilder.Services.RemoveAll<ICache>();
            wizardBuilder.Services.AddSingleton<ICache, DistributedCache>();

            return wizardBuilder;
        }

        public static IImageWizardBuilder SetFileCache(this IImageWizardBuilder wizardBuilder)
        {
            return SetFileCache(wizardBuilder, options => { });
        }

        public static IImageWizardBuilder SetFileCache(this IImageWizardBuilder wizardBuilder, Action<FileCacheSettings> fileCacheSettingsSetup)
        {
            wizardBuilder.Services.Configure(fileCacheSettingsSetup);

            wizardBuilder.Services.RemoveAll<ICache>();
            wizardBuilder.Services.AddSingleton<ICache, FileCache>();

            return wizardBuilder;
        }
    }
}
