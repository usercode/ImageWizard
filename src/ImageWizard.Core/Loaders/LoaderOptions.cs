// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Loaders;

/// <summary>
/// LoaderOptions
/// </summary>
public class LoaderOptions
{
    /// <summary>
    /// Refresh strategy for the original data.
    /// </summary>
    public LoaderRefreshMode RefreshMode { get; set; } = LoaderRefreshMode.None;

    /// <summary>
    /// Overrides max-age value from original source.
    /// </summary>
    public TimeSpan? CacheControlMaxAge { get; set; }

    /// <summary>
    /// Maximum loader source content length (Default: 32 MB)
    /// </summary>
    public long MaxLoaderSourceLength { get; set; } = 1024 * 1024 * 32; //25 MB
}
