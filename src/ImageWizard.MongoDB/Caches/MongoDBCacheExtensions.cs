// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ImageWizard.Caches;

namespace ImageWizard.MongoDB;

public static class MongoDBCacheExtensions
{
    /// <summary>
    /// SetMongoDBCache
    /// </summary>
    /// <param name="wizardConfiguration"></param>
    /// <param name="cacheSettingsSetup"></param>
    /// <returns></returns>
    public static IImageWizardBuilder SetMongoDBCache(this IImageWizardBuilder wizardConfiguration, Action<MongoDBCacheOptions>? cacheSettingsSetup = null)
    {
        if (cacheSettingsSetup != null)
        {
            wizardConfiguration.Services.Configure(cacheSettingsSetup);
        }

        wizardConfiguration.Services.RemoveAll<ICache>();
        wizardConfiguration.Services.AddSingleton<ICache, MongoDBCache>();

        return wizardConfiguration;
    }
}
