using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ImageWizard.Caches;

namespace ImageWizard.MongoDB
{
    public static class MongoDBCacheExtensions
    {
        public static IImageWizardBuilder SetMongoDBCache(this IImageWizardBuilder wizardConfiguration)
        {
            return SetMongoDBCache(wizardConfiguration, options => { });
        }

        public static IImageWizardBuilder SetMongoDBCache(this IImageWizardBuilder wizardConfiguration, Action<MongoDBCacheOptions> cacheSettingsSetup)
        {
            wizardConfiguration.Services.Configure(cacheSettingsSetup);

            wizardConfiguration.Services.RemoveAll<ICache>();
            wizardConfiguration.Services.AddSingleton<ICache, MongoDBCache>();

            return wizardConfiguration;
        }
    }
}
