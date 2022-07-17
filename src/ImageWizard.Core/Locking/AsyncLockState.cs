// Copyright (c) usercode
// https://github.com/usercode/AsyncLock
// MIT License

namespace AsyncLock;

/// <summary>
/// AsyncLockState
/// </summary>
public enum AsyncLockState
{
    /// <summary>
    /// There are currently no running locks.
    /// </summary>
    Idle = 0,

    /// <summary>
    /// There are waiting or running readers.
    /// </summary>
    Reader = 1,

    /// <summary>
    /// There is a waiting or running writer.
    /// </summary>
    Writer = 2
}
