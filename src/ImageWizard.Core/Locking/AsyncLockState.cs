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
/// AsyncLockState
/// </summary>
public class AsyncLockState : IDisposable
{
    public AsyncLockState(AsyncLockReleaser releaser)
    {
        _releaser = releaser; 
        _asyncLock = releaser.AsyncLock;
    }

    private AsyncLockReleaser? _releaser;
    private readonly AsyncLock _asyncLock;
    
    public async Task UpgradeToWriterLockAsync()
    {
        if (_releaser != null)
        {
            _releaser = await _asyncLock.UpgradeToWriterLockAsync(_releaser.Value);
        }
        else
        {
            _releaser = await _asyncLock.WriterLockAsync();
        }
    }

    public async Task DowngradeToReaderLockAsync()
    {
        if (_releaser != null)
        {
            _releaser = await _asyncLock.DowngradeToReaderLockAsync(_releaser.Value);
        }
        else
        {
            _releaser = await _asyncLock.ReaderLockAsync();
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

    public static implicit operator AsyncLockState(AsyncLockReleaser releaser)
    {
        return new AsyncLockState(releaser);
    }
}
