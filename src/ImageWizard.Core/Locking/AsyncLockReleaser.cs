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
public struct AsyncLockReleaser : IDisposable
{
    private readonly AsyncLock _asyncLock;
    private readonly bool _writer;

    internal AsyncLockReleaser(AsyncLock asyncLock, bool writer)
    {
        _asyncLock = asyncLock;
        _writer = writer;
    }

    internal bool Writer => _writer;

    internal AsyncLock AsyncLock => _asyncLock;

    public void Dispose()
    {
        _asyncLock.Release(_writer);
    }
}
