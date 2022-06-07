// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders;

/// <summary>
/// LoaderOptions
/// </summary>
public class LoaderOptions
{
    public LoaderOptions()
    {
        RefreshMode = LoaderRefreshMode.None;
        MaxLoaderSourceLength = 1024 * 1024 * 25; //25 MB
    }

    /// <summary>
    /// Refresh strategy for the original data.
    /// </summary>
    public LoaderRefreshMode RefreshMode { get; set; }

    /// <summary>
    /// Overrides max-age value from original source.
    /// </summary>
    public TimeSpan? CacheControlMaxAge { get; set; }

    /// <summary>
    /// Maximum loader source content length (Default: 25 MB)
    /// </summary>
    public long MaxLoaderSourceLength { get; set; }
}
