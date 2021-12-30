using ImageWizard.Core.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ImageWizard.Caches
{
    public static class FileCacheExtensions
    {
        public static IImageWizardBuilder SetDistributedCache(this IImageWizardBuilder wizardBuilder)
        {
            wizardBuilder.Services.AddDistributedMemoryCache();

            wizardBuilder.Services.RemoveAll<ICache>();
            wizardBuilder.Services.AddTransient<ICache, DistributedCache>();

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
