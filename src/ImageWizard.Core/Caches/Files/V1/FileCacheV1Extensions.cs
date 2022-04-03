// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ImageWizard.Caches;

namespace ImageWizard;

public static class FileCacheV1Extensions
{
    public static IImageWizardBuilder SetDistributedCache(this IImageWizardBuilder wizardBuilder)
    {
        wizardBuilder.Services.RemoveAll<ICache>();
        wizardBuilder.Services.AddSingleton<ICache, DistributedCache>();

        return wizardBuilder;
    }

    /// <summary>
    /// Adds file cache (<see cref="FileCacheV1"/>):<br/>
    /// Meta and blob file path based on cache id.
    /// </summary>
    /// <param name="wizardBuilder"></param>
    /// <returns></returns>
    public static IImageWizardBuilder SetFileCacheV1(this IImageWizardBuilder wizardBuilder)
    {
        return SetFileCacheV1(wizardBuilder, options => { });
    }

    /// <summary>
    /// Adds file cache (<see cref="FileCacheV1">):<br/>
    /// Meta and blob file path based on cache id.
    /// </summary>
    /// <param name="wizardBuilder"></param>
    /// <param name="fileCacheSettingsSetup"></param>
    /// <returns></returns>
    public static IImageWizardBuilder SetFileCacheV1(this IImageWizardBuilder wizardBuilder, Action<FileCacheV1Options> fileCacheSettingsSetup)
    {
        wizardBuilder.Services.Configure(fileCacheSettingsSetup);

        wizardBuilder.Services.RemoveAll<ICache>();
        wizardBuilder.Services.AddSingleton<ICache, FileCacheV1>();

        return wizardBuilder;
    }
}
