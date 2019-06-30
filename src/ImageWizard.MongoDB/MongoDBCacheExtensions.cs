using ImageWizard.Core.Middlewares;
using ImageWizard.ImageStorages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.MongoDB
{
    public static class MongoDBCacheExtensions
    {
        public static IImageWizardBuilder AddMongoDBCache(this IImageWizardBuilder wizardConfiguration, Action<MongoDBCacheSettings> cacheSettingsSetup)
        {
            wizardConfiguration.Services.Configure(cacheSettingsSetup);

            wizardConfiguration.Services.AddSingleton<IImageCache, MongoDBCache>();

            return wizardConfiguration;
        }
    }
}
