// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ImageWizard;

public static class CleanupExtensions
{
    /// <summary>
    /// Adds a background service which removes cached data based on defined reasons.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IImageWizardBuilder AddCleanupService(this IImageWizardBuilder builder, Action<CleanupOptions> options)
    {
        builder.Services.Configure(options);
        builder.Services.AddHostedService<CleanupBackgroundService>();

        return builder;
    }

    /// <summary>
    /// Removes cached data which are older than defined duration.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static CleanupOptions OlderThan(this CleanupOptions options, TimeSpan duration)
    {
        options.Reasons.Add(new OlderThanReason(duration));

        return options;
    }

    /// <summary>
    /// Removes cached data which are last used since defined duration.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static CleanupOptions LastUsedSince(this CleanupOptions options, TimeSpan duration)
    {
        options.Reasons.Add(new LastUsedSinceReason(duration));

        return options;
    }
}
