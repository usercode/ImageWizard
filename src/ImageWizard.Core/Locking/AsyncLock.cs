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
public class AsyncLock
{
    public AsyncLock()
    {
        _syncObj = _waitingWriters;

        _readerReleaser = Task.FromResult(new AsyncLockReleaser(this, AsyncLockType.Read));
        _writerReleaser = Task.FromResult(new AsyncLockReleaser(this, AsyncLockType.Write));
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
    internal int CountRunningReaders => _readersRunning;

    /// <summary>
    /// CountWaitingReaders
    /// </summary>
    internal int CountWaitingReaders => _waitingReaders.Count;

    /// <summary>
    /// IsWriterRunning
    /// </summary>
    internal bool IsWriterRunning => _isWriterRunning;

    /// <summary>
    /// CountWaitingWriters
    /// </summary>
    internal int CountWaitingWriters => _waitingWriters.Count;

    /// <summary>
    /// State
    /// </summary>
    public AsyncLockState State
    {
        get
        {
            if (IsWriterRunning)
                return AsyncLockState.Writer;
            else if (CountRunningReaders > 0)
                return AsyncLockState.Reader;
            else
                return AsyncLockState.Idle;
        }
    }

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

    internal Task<AsyncLockReleaser> SwitchToWriterLockAsync(AsyncLockReleaser releaser, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
        {
            return Task.FromCanceled<AsyncLockReleaser>(cancellation);
        }

        lock (_syncObj)
        {
            if (releaser.Type == AsyncLockType.Write)
            {
                throw new Exception("There is already a writer lock.");
            }

            var taskWriter = WriterLockAsync(cancellation);

            if (releaser.Type != null)
            {
                Release(releaser.Type.Value, sendReleasedEvent: false);
            }

            return taskWriter;
        }
    }

    internal Task<AsyncLockReleaser> SwitchToReaderLockAsync(AsyncLockReleaser releaser, bool skipWaitingWriters = false, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
        {
            return Task.FromCanceled<AsyncLockReleaser>(cancellation);
        }

        lock (_syncObj)
        {
            if (releaser.Type == AsyncLockType.Read)
            {
                throw new Exception("There is already a reader lock.");
            }

            var taskReader = ReaderLockAsync(cancellation);

            if (releaser.Type != null)
            {
                Release(releaser.Type.Value, skipWaitingWriters: skipWaitingWriters, sendReleasedEvent: false);
            }

            return taskReader;
        }
    }

    internal void Release(AsyncLockType type, bool skipWaitingWriters = false, bool sendReleasedEvent = true)
    {
        lock (_syncObj)
        {
            try
            {
                if (type == AsyncLockType.Write)
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
        _readersRunning--;

        //start next waiting writer lock
        if (_readersRunning == 0)
        {
            StartNextWaitingWriter();
        }
    }

    private void WriterRelease(bool skipWaitingWriters)
    {
        _isWriterRunning = false;

        //start next writer lock?
        if (skipWaitingWriters == false)
        {
            StartNextWaitingWriter();
        }

        //no running writer lock?
        if (_isWriterRunning == false)
        {
            while (_waitingReaders.Count > 0)
            {
                var taskSource = _waitingReaders.Dequeue();

                bool result = taskSource.TrySetResult(new AsyncLockReleaser(this, AsyncLockType.Read));

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

            bool result = taskSource.TrySetResult(new AsyncLockReleaser(this, AsyncLockType.Write));

            if (result == true)
            {
                _isWriterRunning = true;

                return;
            }
        }

        _isWriterRunning = false;

        return;
    }
}
