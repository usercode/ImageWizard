// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.Locking;

/// <summary>
/// AsyncLockKey
/// </summary>
public class AsyncLock<TKey>
    where TKey : notnull
{
    public AsyncLock()
    {
        _locks = new Dictionary<TKey, AsyncLock>();
    }

    private readonly IDictionary<TKey, AsyncLock> _locks;

    /// <summary>
    /// ReaderLockAsync
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<AsyncLockReleaser> ReaderLockAsync(TKey key)
    {
        lock (_locks)
        {
            if (_locks.TryGetValue(key, out AsyncLock? asyncLock) == false)
            {
                asyncLock = new AsyncLock(_locks);

                _locks.Add(key, asyncLock);
            }

            return asyncLock.ReaderLockAsync();
        }
    }

    /// <summary>
    /// WriterLockAsync
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<AsyncLockReleaser> WriterLockAsync(TKey key)
    {
        lock (_locks)
        {
            if (_locks.TryGetValue(key, out AsyncLock? asyncLock) == false)
            {
                asyncLock = new AsyncLock(_locks);
                asyncLock.Released += x => 
                {
                    lock (_locks)
                    {
                        if (x.IsIdle)
                        {
                            _locks.Remove(key);
                        }
                    }
                };

                _locks.Add(key, asyncLock);
            }

            return asyncLock.WriterLockAsync();
        }
    }
}
