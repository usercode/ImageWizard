// Copyright (c) usercode
// https://github.com/usercode/AsyncLock
// MIT License

namespace AsyncLock;

/// <summary>
/// AsyncLockReleaser
/// </summary>
public class AsyncLockReleaser : IDisposable
{
    private readonly AsyncLock _asyncLock;
    private readonly AsyncLockType _type;

    internal AsyncLockReleaser(AsyncLock asyncLock, AsyncLockType type)
    {
        _asyncLock = asyncLock;
        _type = type;
    }

    internal AsyncLockType Type => _type;

    internal AsyncLock AsyncLock => _asyncLock;

    private bool _disposed;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _asyncLock.Release(_type);
        
        _disposed = true;

        GC.SuppressFinalize(this);
    }
}
