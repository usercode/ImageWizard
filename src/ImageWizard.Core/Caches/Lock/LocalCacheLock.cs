// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Locking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

/// <summary>
/// LocalCacheLock
/// </summary>
public class LocalCacheLock : ICacheLock
{
    private readonly AsyncLock<string> AsyncLock = new AsyncLock<string>();

    public async Task<IDisposable> ReaderLockAsync(string key, CancellationToken cancellation)
    {
        return await AsyncLock.ReaderLockAsync(key, cancellation);
    }

    public async Task<IDisposable> WriterLockAsync(string key, CancellationToken cancellation)
    {
        return await AsyncLock.WriterLockAsync(key, cancellation);
    }
}
