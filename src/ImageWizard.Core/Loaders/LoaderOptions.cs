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
    }

    /// <summary>
    /// Refresh strategy for the original data.
    /// </summary>
    public LoaderRefreshMode RefreshMode { get; set; }

    /// <summary>
    /// Overrides max-age value from original source.
    /// </summary>
    public TimeSpan? CacheControlMaxAge { get; set; }
}
