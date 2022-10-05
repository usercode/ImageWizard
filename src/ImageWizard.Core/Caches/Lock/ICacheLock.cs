// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Caches;

/// <summary>
/// ICacheLock
/// </summary>
public interface ICacheLock
{
    Task<IDisposable> ReaderLockAsync(string key, CancellationToken cancellation = default);

    Task<IDisposable> WriterLockAsync(string key, CancellationToken cancellation = default);
}
