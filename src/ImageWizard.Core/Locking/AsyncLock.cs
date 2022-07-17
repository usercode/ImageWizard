// Copyright (c) usercode
// https://github.com/usercode/AsyncLock
// MIT License

namespace AsyncLock;

/// <summary>
/// AsyncLock
/// </summary>
public class AsyncLock
{
    public AsyncLock()
    {
        _syncObj = _waitingWriters;
    }

    internal AsyncLock(object syncObject)
    {
        _syncObj = syncObject;
    }

    private readonly object _syncObj;

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
            {
                return AsyncLockState.Writer;
            }
            else if (CountRunningReaders > 0)
            {
                return AsyncLockState.Reader;
            }
            else
            {
                return AsyncLockState.Idle;
            }
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
                return Task.FromResult(new AsyncLockReleaser(this, AsyncLockType.Read));
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
                return Task.FromResult(new AsyncLockReleaser(this, AsyncLockType.Write));
            }
            else
            {
                //create waiting writer
                TaskCompletionSource<AsyncLockReleaser> waiter = _waitingWriters.Enqueue(cancellation);
                return waiter.Task;
            }
        }
    }

    internal void Release(AsyncLockType type, bool sendReleasedEvent = true)
    {
        lock (_syncObj)
        {
            try
            {
                if (type == AsyncLockType.Write)
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
