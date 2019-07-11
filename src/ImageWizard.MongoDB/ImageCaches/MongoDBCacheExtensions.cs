using ImageWizard.Core.Middlewares;
using ImageWizard.ImageStorages;
using ImageWizard.MongoDB.ImageCaches;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ImageWizard.MongoDB
{
    public static class MongoDBCacheExtensions
    {
        public static IImageWizardBuilder SetMongoDBCache(this IImageWizardBuilder wizardConfiguration)
        {
            return SetMongoDBCache(wizardConfiguration, options => { });
        }

        public static IImageWizardBuilder SetMongoDBCache(this IImageWizardBuilder wizardConfiguration, Action<MongoDBCacheSettings> cacheSettingsSetup)
        {
            wizardConfiguration.Services.Configure(cacheSettingsSetup);

            wizardConfiguration.Services.RemoveAll<IImageCache>();
            wizardConfiguration.Services.AddSingleton<IImageCache, MongoDBCache>();

            return wizardConfiguration;
        }
    }
}
