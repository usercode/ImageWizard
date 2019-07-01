using ImageWizard.Core.ImageCaches;
using ImageWizard.Core.Middlewares;
using ImageWizard.ImageStorages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class FileCacheExtensions
    {
        public static IImageWizardBuilder AddDistributedCache(this IImageWizardBuilder wizardBuilder)
        {
            wizardBuilder.Services.AddTransient<IImageCache, DistributedCache>();

            return wizardBuilder;
        }

        public static IImageWizardBuilder AddFileCache(this IImageWizardBuilder wizardBuilder)
        {
            return AddFileCache(wizardBuilder, options => { });
        }

        public static IImageWizardBuilder AddFileCache(this IImageWizardBuilder wizardBuilder, Action<FileCacheSettings> fileCacheSettingsSetup)
        {
            wizardBuilder.Services.Configure(fileCacheSettingsSetup);

            wizardBuilder.Services.AddSingleton<IImageCache, FileCache>();

            return wizardBuilder;
        }
    }
}
