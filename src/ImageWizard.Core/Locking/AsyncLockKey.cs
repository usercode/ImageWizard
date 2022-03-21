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
/// KeybasedAsyncLock
/// </summary>
public class AsyncLockKey
{
    public AsyncLockKey()
    {
        _locks = new Dictionary<string, AsyncLock>();
    }

    private readonly IDictionary<string, AsyncLock> _locks;

    public AsyncLock GetLock(string key)
    {
        lock (_locks)
        {
            if (_locks.TryGetValue(key, out AsyncLock? asyncLock) == false)
            {
                asyncLock = new AsyncLock();

                _locks.Add(key, asyncLock);
            }

            return asyncLock;
        }
    }

    public void ReleaseLock(AsyncLock asyncLock)
    {
        lock (_locks)
        {
            _locks.Remove("");
        }
    }
}
