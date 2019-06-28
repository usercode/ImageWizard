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
        public static IImageWizardBuilder AddFileCache(this IImageWizardBuilder wizardConfiguration, FileCacheSettings fileCacheSettings)
        {
            wizardConfiguration.Services.AddSingleton(fileCacheSettings);
            wizardConfiguration.Services.AddSingleton<IImageCache, FileCache>();

            return wizardConfiguration;
        }
    }
}
