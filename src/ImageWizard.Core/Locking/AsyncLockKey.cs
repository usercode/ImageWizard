﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWizard.Core.Locking;

/// <summary>
/// AsyncLockKey
/// </summary>
public class AsyncLock<TKey>
    where TKey : notnull
{
    private readonly IDictionary<TKey, AsyncLock> _locks = new Dictionary<TKey, AsyncLock>();

    /// <summary>
    /// GetAsyncLock
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private AsyncLock GetAsyncLock(TKey key)
    {
        if (_locks.TryGetValue(key, out AsyncLock? asyncLock) == false)
        {
            asyncLock = new AsyncLock(_locks);
            asyncLock.Released += x =>
            {
                if (x.IsIdle)
                {
                    _locks.Remove(key);
                }
            };

            _locks.Add(key, asyncLock);
        }

        return asyncLock;
    }

    /// <summary>
    /// ReaderLockAsync
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<AsyncLockReleaser> ReaderLockAsync(TKey key, CancellationToken cancellation = default)
    {
        lock (_locks)
        {
            return GetAsyncLock(key).ReaderLockAsync(cancellation);
        }
    }

    /// <summary>
    /// WriterLockAsync
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<AsyncLockReleaser> WriterLockAsync(TKey key, CancellationToken cancellation = default)
    {
        lock (_locks)
        {
            return GetAsyncLock(key).WriterLockAsync(cancellation);
        }
    }
}
