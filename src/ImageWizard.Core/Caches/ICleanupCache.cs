// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;

namespace ImageWizard.Caches;

/// <summary>
/// A cache with cleanup support.
/// </summary>
public interface ICleanupCache : ICache
{
    /// <summary>
    /// CleanupAsync
    /// </summary>
    Task CleanupAsync(CleanupReason reason, CancellationToken cancellationToken = default);
}
