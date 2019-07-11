using ImageWizard.Core.ImageCaches;
using ImageWizard.Core.Middlewares;
using ImageWizard.ImageStorages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ImageWizard
{
    public static class FileCacheExtensions
    {
        public static IImageWizardBuilder SetDistributedCache(this IImageWizardBuilder wizardBuilder)
        {
            wizardBuilder.Services.AddDistributedMemoryCache();

            wizardBuilder.Services.RemoveAll<IImageCache>();
            wizardBuilder.Services.AddTransient<IImageCache, DistributedCache>();

            return wizardBuilder;
        }

        public static IImageWizardBuilder SetFileCache(this IImageWizardBuilder wizardBuilder)
        {
            return SetFileCache(wizardBuilder, options => { });
        }

        public static IImageWizardBuilder SetFileCache(this IImageWizardBuilder wizardBuilder, Action<FileCacheSettings> fileCacheSettingsSetup)
        {
            wizardBuilder.Services.Configure(fileCacheSettingsSetup);

            wizardBuilder.Services.RemoveAll<IImageCache>();
            wizardBuilder.Services.AddSingleton<IImageCache, FileCache>();

            return wizardBuilder;
        }
    }
}
