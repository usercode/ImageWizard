// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

/// <summary>
/// A cache with cleanup support.
/// </summary>
public interface ICleanupCache : ICache
{
    /// <summary>
    /// CleanUpAsync
    /// </summary>
    /// <returns></returns>
    Task CleanupAsync(IEnumerable<CleanupReason> reasons, CancellationToken cancellationToken = default);


}
