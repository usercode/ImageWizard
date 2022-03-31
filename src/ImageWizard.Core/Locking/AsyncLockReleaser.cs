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
/// AsyncLockReleaser
/// </summary>
public readonly struct AsyncLockReleaser : IDisposable
{
    private readonly AsyncLock _asyncLock;
    private readonly AsyncLockType? _type;

    internal AsyncLockReleaser(AsyncLock asyncLock, AsyncLockType? type)
    {
        _asyncLock = asyncLock;
        _type = type;
    }

    internal AsyncLockType? Type => _type;

    internal AsyncLock AsyncLock => _asyncLock;

    public void Dispose()
    {
        if (_type != null)
        {
            _asyncLock.Release(_type.Value);
        }
    }
}
