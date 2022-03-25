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

        _readerReleaser = Task.FromResult(new AsyncLockReleaser(this, false));
        _writerReleaser = Task.FromResult(new AsyncLockReleaser(this, true));
    }

    internal AsyncLock(object syncObject)
        : this()
    {
        _syncObj = syncObject;
    }

    private readonly object _syncObj;

    private readonly Task<AsyncLockReleaser> _readerReleaser;
    private readonly Task<AsyncLockReleaser> _writerReleaser;

    private readonly Queue<TaskCompletionSource<AsyncLockReleaser>> _waitingReaders = new Queue<TaskCompletionSource<AsyncLockReleaser>>();
    private readonly Queue<TaskCompletionSource<AsyncLockReleaser>> _waitingWriters = new Queue<TaskCompletionSource<AsyncLockReleaser>>();
    
    private int _readersRunning;

    private bool _isWriterRunning = false;

    public event Action<AsyncLock>? Released;

    /// <summary>
    /// CountRunningReaders
    /// </summary>
    public int CountRunningReaders => _readersRunning;

    /// <summary>
    /// CountWaitingReaders
    /// </summary>
    public int CountWaitingReaders => _waitingReaders.Count;

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

    public Task<AsyncLockReleaser> ReaderLockAsync(CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
        {
            return Task.FromCanceled<AsyncLockReleaser>(cancellation);
        }

        lock (_syncObj)
        {
            //no running or waiting write lock?
            if (_isWriterRunning == false && _waitingWriters.Count == 0)
            {
                _readersRunning++;
                return _readerReleaser;
            }
            else
            {
                //create waiting reader
                TaskCompletionSource<AsyncLockReleaser> waiter = _waitingReaders.Enqueue(cancellation);
                return waiter.Task;
            }
        }
    }

    public Task<AsyncLockReleaser> WriterLockAsync(CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
        {
            return Task.FromCanceled<AsyncLockReleaser>(cancellation);
        }

        lock (_syncObj)
        {
            if (_isWriterRunning == false && _readersRunning == 0)
            {
                _isWriterRunning = true;
                return _writerReleaser;
            }
            else
            {
                //create waiting writer
                TaskCompletionSource<AsyncLockReleaser> waiter = _waitingWriters.Enqueue(cancellation);
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

            Release(releaser.Writer, false);

            return WriterLockAsync();
        }
    }

    internal Task<AsyncLockReleaser> DowngradeToReaderLockAsync(AsyncLockReleaser releaser)
    {
        lock (_syncObj)
        {
            if (releaser.Writer == false)
            {
                throw new Exception("There is already a reader lock.");
            }

            Release(releaser.Writer, false);

            return ReaderLockAsync();
        }
    }

    internal void Release(bool writer, bool sendReleasedEvent = true)
    {
        lock (_syncObj)
        {
            try
            {
                if (writer == true)
                {
                    WriterRelease();
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
        _readersRunning--;

        //start next waiting writer lock
        if (_readersRunning == 0)
        {
            StartNextWaitingWriter();
        }
    }

    private void WriterRelease()
    {
        //start next writer lock?
        StartNextWaitingWriter();

        //no running writer lock?
        if (_isWriterRunning == false)
        {
            while (_waitingReaders.Count > 0)
            {
                var taskSource = _waitingReaders.Dequeue();

                bool result = taskSource.TrySetResult(new AsyncLockReleaser(this, false));

                if (result)
                {
                    _readersRunning++;
                }
            }
        }
    }

    private void StartNextWaitingWriter()
    {
        while (_waitingWriters.Count > 0)
        {
            var taskSource = _waitingWriters.Dequeue();

            bool result = taskSource.TrySetResult(new AsyncLockReleaser(this, true));

            if (result == true)
            {
                _isWriterRunning = true;

                return;
            }
        }

        _isWriterRunning = false;

        return;
    }

    public void Dispose()
    {

    }
}
