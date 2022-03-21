// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders;

/// <summary>
/// Refresh strategy for the original data.
/// </summary>
public enum LoaderRefreshMode
{
    /// <summary>
    /// The cached data will be never refreshed. (Recommend for fingerprint strategy.)
    /// </summary>
    None = 0,

    /// <summary>
    /// Checks every time for a new version of the original data.
    /// </summary>
    EveryTime = 1,

    /// <summary>
    /// Use cache control information of the cache data.
    /// </summary>
    BasedOnCacheControl = 2
}
