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
    /// RefreshMode
    /// </summary>
    public LoaderRefreshMode RefreshMode { get; set; }

    /// <summary>
    /// CacheControl
    /// </summary>
    public TimeSpan? CacheControlMaxAge { get; set; }
}
