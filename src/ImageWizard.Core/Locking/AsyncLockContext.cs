// Copyright (c) usercode
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
/// AsyncLockState
/// </summary>
public class AsyncLockContext : IDisposable
{
    public AsyncLockContext(AsyncLockReleaser releaser)
    {
        _releaser = releaser; 
        _asyncLock = releaser.AsyncLock;
    }

    private AsyncLockReleaser? _releaser;
    private readonly AsyncLock _asyncLock;

    public async Task SwitchToWriterLockAsync(CancellationToken cancellation = default)
    {
        if (_releaser != null)
        {
            _releaser = await _asyncLock.SwitchToWriterLockAsync(_releaser.Value, cancellation);
        }
        else
        {
            _releaser = await _asyncLock.WriterLockAsync(cancellation);
        }
    }

    public async Task SwitchToReaderLockAsync(bool skipWaitingWriters = false, CancellationToken cancellation = default)
    {
        if (_releaser != null)
        {
            _releaser = await _asyncLock.SwitchToReaderLockAsync(_releaser.Value, skipWaitingWriters, cancellation);
        }
        else
        {
            _releaser = await _asyncLock.ReaderLockAsync(cancellation);
        }
    }

    public void Dispose()
    {
        if (_releaser != null)
        {
            _releaser.Value.Dispose();
            _releaser = null;
        }

        GC.SuppressFinalize(this);
    }

    public static implicit operator AsyncLockContext(AsyncLockReleaser releaser)
    {
        return new AsyncLockContext(releaser);
    }
}
