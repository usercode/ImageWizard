﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ImageWizard.Caches;

namespace ImageWizard;

public static class FileCacheExtensions
{
    public static IImageWizardBuilder SetDistributedCache(this IImageWizardBuilder wizardBuilder)
    {
        wizardBuilder.Services.RemoveAll<ICache>();
        wizardBuilder.Services.AddSingleton<ICache, DistributedCache>();

        return wizardBuilder;
    }

    /// <summary>
    /// Adds file cache (<see cref="FileCacheV1">):<br/>
    /// Meta and blob file path based on cache id.
    /// </summary>
    /// <param name="wizardBuilder"></param>
    /// <param name="fileCacheSettingsSetup"></param>
    /// <returns></returns>
    public static IImageWizardBuilder SetFileCache(this IImageWizardBuilder wizardBuilder, Action<FileCacheOptions>? fileCacheSettingsSetup = null)
    {
        if (fileCacheSettingsSetup != null)
        {
            wizardBuilder.Services.Configure(fileCacheSettingsSetup);
        }

        wizardBuilder.Services.RemoveAll<ICache>();
        wizardBuilder.Services.AddSingleton<ICache, FileCache>();

        return wizardBuilder;
    }
}