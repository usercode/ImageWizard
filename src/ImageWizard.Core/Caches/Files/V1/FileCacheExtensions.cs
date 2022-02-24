// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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

        /// <summary>
        /// Adds file cache (<see cref="FileCache"/>):<br/>
        /// Meta and blob file path based on cache id.
        /// </summary>
        /// <param name="wizardBuilder"></param>
        /// <returns></returns>
        public static IImageWizardBuilder SetFileCache(this IImageWizardBuilder wizardBuilder)
        {
            return SetFileCache(wizardBuilder, options => { });
        }

        /// <summary>
        /// Adds file cache (<see cref="FileCache">):<br/>
        /// Meta and blob file path based on cache id.
        /// </summary>
        /// <param name="wizardBuilder"></param>
        /// <param name="fileCacheSettingsSetup"></param>
        /// <returns></returns>
        public static IImageWizardBuilder SetFileCache(this IImageWizardBuilder wizardBuilder, Action<FileCacheSettings> fileCacheSettingsSetup)
        {
            wizardBuilder.Services.Configure(fileCacheSettingsSetup);

            wizardBuilder.Services.RemoveAll<ICache>();
            wizardBuilder.Services.AddSingleton<ICache, FileCache>();

            return wizardBuilder;
        }
    }
}
