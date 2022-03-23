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
        _syncObj = _waitingWriters;

        m_readerReleaser = Task.FromResult(new AsyncLockReleaser(this, false));
        m_writerReleaser = Task.FromResult(new AsyncLockReleaser(this, true));
    }

    internal AsyncLock(object syncObject)
        : this()
    {
        _syncObj = syncObject;
    }

    private readonly Task<AsyncLockReleaser> m_readerReleaser;
    private readonly Task<AsyncLockReleaser> m_writerReleaser;

    public event Action<AsyncLock>? Released;

    private readonly Queue<TaskCompletionSource<AsyncLockReleaser>> _waitingWriters = new Queue<TaskCompletionSource<AsyncLockReleaser>>();

    private TaskCompletionSource<AsyncLockReleaser> _waitingReader = new TaskCompletionSource<AsyncLockReleaser>();
    
    private int _readersWaiting;
    private int _readersRunning;

    private bool _isWriterRunning = false;

    private readonly object _syncObj;

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

    /// <summary>
    /// CountWaitingWriters
    /// </summary>
    public int CountWaitingWriters => _waitingWriters.Count;

    /// <summary>
    /// Are there any waiting or running locks?
    /// </summary>
    public bool IsIdle => CountRunningReaders == 0 && CountWaitingReaders == 0 && IsWriterRunning == false && CountWaitingWriters == 0;

    public Task<AsyncLockReleaser> ReaderLockAsync()
    {
        lock (_syncObj)
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
        lock (_syncObj)
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

    internal Task<AsyncLockReleaser> UpgradeToWriterLockAsync(AsyncLockReleaser releaser)
    {
        lock (_syncObj)
        {
            if (releaser.Writer == true)
            {
                throw new Exception("There is already a writer lock.");
            }

            Task<AsyncLockReleaser> writerTask = WriterLockAsync();

            Release(releaser.Writer, false, false);

            return writerTask;
        }
    }

    internal Task<AsyncLockReleaser> DowngradeToReaderLockAsync(AsyncLockReleaser releaser, bool skipWaitingWriters = false)
    {
        lock (_syncObj)
        {
            if (releaser.Writer == false)
            {
                throw new Exception("There is already a reader lock.");
            }

            Task<AsyncLockReleaser> readerTask = ReaderLockAsync();

            Release(releaser.Writer, false, skipWaitingWriters);

            return readerTask;
        }
    }

    internal void Release(bool writer, bool sendReleasedEvent = true, bool skipWaitingWriters = false)
    {
        lock (_syncObj)
        {
            try
            {
                if (writer == true)
                {
                    WriterRelease(skipWaitingWriters);
                }
                else
                {
                    ReaderRelease();
                }
            }
            finally
            {
                if (sendReleasedEvent)
                {
                    Released?.Invoke(this);
                }
            }
        }
    }

    private void ReaderRelease()
    {
        TaskCompletionSource<AsyncLockReleaser>? toWake = null;

        lock (_syncObj)
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

    private void WriterRelease(bool skipWaitingWriters = false)
    {
        TaskCompletionSource<AsyncLockReleaser>? toWake = null;
        bool toWakeIsWriter = false;

        lock (_syncObj)
        {
            //use next write lock?
            if (skipWaitingWriters == false && _waitingWriters.Count > 0)
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
