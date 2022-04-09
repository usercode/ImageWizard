// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

/// <summary>
/// ICacheLock
/// </summary>
public interface ICacheLock
{
    Task<IDisposable> ReaderLockAsync(string key, CancellationToken cancellation = default);

    Task<IDisposable> WriterLockAsync(string key, CancellationToken cancellation = default);
}
