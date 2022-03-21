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
/// AsyncLock
/// </summary>
public class AsyncLock : IDisposable
{
    public AsyncLock()
    {
        m_readerReleaser = Task.FromResult(new AsyncLockReleaser(this, false));
        m_writerReleaser = Task.FromResult(new AsyncLockReleaser(this, true));
    }

    private readonly Task<AsyncLockReleaser> m_readerReleaser;
    private readonly Task<AsyncLockReleaser> m_writerReleaser;

    private readonly Queue<TaskCompletionSource<AsyncLockReleaser>> _waitingWriters = new Queue<TaskCompletionSource<AsyncLockReleaser>>();

    private TaskCompletionSource<AsyncLockReleaser> _waitingReader = new TaskCompletionSource<AsyncLockReleaser>();
    
    private int _readersWaiting;
    private int _readersRunning;

    private bool _isWriterRunning = false;

    /// <summary>
    /// CountRunningReaders
    /// </summary>
    public int CountRunningReaders => _readersRunning;

    /// <summary>
    /// CountWaitingReaders
    /// </summary>
    public int CountWaitingReaders => _readersWaiting;

    /// <summary>
    /// IsWriterRunning
    /// </summary>
    public bool IsWriterRunning => _isWriterRunning;

    public Task<AsyncLockReleaser> ReaderLockAsync()
    {
        lock (_waitingWriters)
        {
            //no active or waiting write lock?
            if (_isWriterRunning == false && _waitingWriters.Count == 0)
            {
                _readersRunning++;
                return m_readerReleaser;
            }
            else
            {
                //return waiting reader state
                _readersWaiting++;
                return _waitingReader.Task;
            }
        }
    }

    public Task<AsyncLockReleaser> WriterLockAsync()
    {
        lock (_waitingWriters)
        {
            if (_isWriterRunning == false && _readersRunning == 0)
            {
                _isWriterRunning = true;
                return m_writerReleaser;
            }
            else
            {
                var waiter = new TaskCompletionSource<AsyncLockReleaser>();
                _waitingWriters.Enqueue(waiter);
                return waiter.Task;
            }
        }
    }

    internal void ReaderRelease()
    {
        TaskCompletionSource<AsyncLockReleaser>? toWake = null;

        lock (_waitingWriters)
        {
            _readersRunning--;

            //start next waiting write lock
            if (_readersRunning == 0 && _waitingWriters.Count > 0)
            {
                _isWriterRunning = true;
                toWake = _waitingWriters.Dequeue();
            }
        }

        if (toWake != null)
        {
            toWake.SetResult(new AsyncLockReleaser(this, true));
        }
    }

    internal async Task DowngradeToReader(AsyncLockReleaser releaser)
    {
        Task readerTask = ReaderLockAsync();

        WriterRelease(true);

        releaser.IsWriter = false;

        await readerTask;
    }

    internal void WriterRelease(bool skipWaitingWriters = false)
    {
        TaskCompletionSource<AsyncLockReleaser>? toWake = null;
        bool toWakeIsWriter = false;

        lock (_waitingWriters)
        {
            //use next write lock?
            if (_waitingWriters.Count > 0)
            {
                toWake = _waitingWriters.Dequeue();
                toWakeIsWriter = true;
            }//use next read lock
            else if (_readersWaiting > 0)
            {
                toWake = _waitingReader;
                _readersRunning = _readersWaiting;
                _readersWaiting = 0;
                _isWriterRunning = false;
                _waitingReader = new TaskCompletionSource<AsyncLockReleaser>();
            }//reset state
            else
            {
                _isWriterRunning = false;
            }
        }

        if (toWake != null)
        {
            toWake.SetResult(new AsyncLockReleaser(this, toWakeIsWriter));
        }
    }

    public void Dispose()
    {

    }
}
