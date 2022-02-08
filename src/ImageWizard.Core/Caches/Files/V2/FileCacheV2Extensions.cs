using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ImageWizard.Caches;

namespace ImageWizard
{
    public static class FileCacheV2Extensions
    {
        /// <summary>
        /// Adds file cache with data deduplication.
        /// </summary>
        /// <param name="wizardBuilder"></param>
        /// <returns></returns>
        public static IImageWizardBuilder SetFileCacheV2(this IImageWizardBuilder wizardBuilder)
        {
            return SetFileCacheV2(wizardBuilder, options => { });
        }

        /// <summary>
        /// Adds file cache with data deduplication.
        /// </summary>
        /// <param name="wizardBuilder"></param>
        /// <param name="fileCacheSettingsSetup"></param>
        /// <returns></returns>
        public static IImageWizardBuilder SetFileCacheV2(this IImageWizardBuilder wizardBuilder, Action<FileCacheV2Settings> fileCacheSettingsSetup)
        {
            wizardBuilder.Services.Configure(fileCacheSettingsSetup);

            wizardBuilder.Services.RemoveAll<ICache>();
            wizardBuilder.Services.AddSingleton<ICache, FileCacheV2>();

            return wizardBuilder;
        }
    }
}
