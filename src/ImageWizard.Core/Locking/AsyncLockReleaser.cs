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
    private readonly AsyncLock _toRelease;
    private bool _writer;
    private bool _disposed = false;

    /// <summary>
    /// IsWriter
    /// </summary>
    public bool IsWriter
    {
        get => _writer;
        internal set => _writer = value;
    }

    internal AsyncLockReleaser(AsyncLock toRelease, bool writer)
    {
        _toRelease = toRelease;
        _writer = writer;
    }

    public void UpgradeToWriteLock()
    {
        _toRelease.WriterLockAsync();
    }

    public Task DowngradeToReadLockAsync()
    {
        return _toRelease.DowngradeToReader(this);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (_writer)
        {
            _toRelease.WriterRelease();
        }
        else
        {
            _toRelease.ReaderRelease();
        }

        _disposed = true;
    }
}
