// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;
using ImageWizard.Caches;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

public static class CleanupExtensions
{
    /// <summary>
    /// Adds a background service which removes cached data based on defined <see cref="CleanupReason"/>.
    /// <br /><br />
    /// The cache needs to implements <see cref="ICleanupCache"/>.
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
    /// <br /><br />
    /// This feature works only if the cache implements <see cref="ILastAccessCache"/>.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static CleanupOptions LastUsedSince(this CleanupOptions options, TimeSpan duration)
    {
        options.Reasons.Add(new LastUsedSinceReason(duration));

        return options;
    }

    /// <summary>
    /// Removes cached data which are expired.
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static CleanupOptions Expired(this CleanupOptions options)
    {
        options.Reasons.Add(new ExpiredReason());

        return options;
    }
}
