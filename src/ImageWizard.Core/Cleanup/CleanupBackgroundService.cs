// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Caches;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageWizard.Cleanup;

/// <summary>
/// CleanupBackgroundService
/// </summary>
public class CleanupBackgroundService : BackgroundService
{
    public CleanupBackgroundService(ICache cache, IOptions<CleanupOptions> options, ILogger<CleanupBackgroundService> logger)
    {
        Cache = cache;
        Options = options.Value;
        Logger = logger;
    }

    /// <summary>
    /// Logger
    /// </summary>
    private ILogger<CleanupBackgroundService> Logger { get; }

    /// <summary>
    /// Cache
    /// </summary>
    private ICache Cache { get; }

    /// <summary>
    /// Options
    /// </summary>
    private CleanupOptions Options { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (Options.Reasons.Count == 0)
        {
            return;
        }

        if (Cache is not ICleanupCache cleanupCache)
        {
            Logger.LogWarning("Cache doesn't support cleanup mode.");

            return;
        }

        while (true)
        {
            stoppingToken.ThrowIfCancellationRequested();

            try
            {
                Logger.LogInformation("Starting cleanup service.");

                foreach (CleanupReason reason in Options.Reasons)
                {
                    if (reason.CanUse(Cache) == false)
                    {
                        Logger.LogWarning("Cleanup reason isn't supported by cache: {reason}", reason.Name);

                        continue;
                    }

                    await cleanupCache.CleanupAsync(reason, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Cleanup job is failed");
            }
            finally
            {
                Logger.LogInformation($"Waits {Options.Interval} for next cleanup.");

                await Task.Delay(Options.Interval, stoppingToken);
            }
        }        
    }
}
