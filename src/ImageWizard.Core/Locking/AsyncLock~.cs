// Copyright (c) usercode
// https://github.com/usercode/AsyncLock
// MIT License

namespace AsyncLock;

/// <summary>
/// AsyncLock
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
                lock (_locks)
                {
                    if (x.State == AsyncLockState.Idle)
                    {
                        _locks.Remove(key);
                    }
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
