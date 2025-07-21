// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;

namespace ImageWizard;

public delegate CachedData? FallbackHandler(LoaderResultState state, ImageWizardUrl url, CachedData? existingCachedData);

/// <summary>
/// ImageWizardOptions
/// </summary>
public class ImageWizardOptions : ImageWizardBaseOptions
{
    /// <summary>
    /// CacheControl
    /// </summary>
    public CacheControl CacheControl { get; } = new CacheControl();

    /// <summary>
    /// Allows unsafe url.
    /// </summary>
    public bool AllowUnsafeUrl { get; set; } = false;

    /// <summary>
    /// Selects automatically the compatible mime type by request header. (Default: false)
    /// </summary>
    public bool UseAcceptHeader { get; set; } = false;

    /// <summary>
    /// Uses ETag. (Default: true)
    /// </summary>
    public bool UseETag { get; set; } = true;

    /// <summary>
    /// Use client hints. (Default: false)
    /// </summary>
    public bool UseClientHints { get; set; } = false;

    /// <summary>
    /// Use cache for original data. (Default: true)
    /// </summary>
    public bool UseLoaderCache { get; set; } = false;

    /// <summary>
    /// Duration when the last-access time (metadata) should be refreshed.
    /// </summary>
    public TimeSpan? RefreshLastAccessInterval { get; set; } = TimeSpan.FromDays(1);

    /// <summary>
    /// Allowed DPR values. (Default: 1.0, 1.5, 2.0, 3.0)
    /// </summary>
    public double[] AllowedDPR { get; set; } = ImageWizardDefaults.AllowedDPR;

    /// <summary>
    /// FallbackHandler
    /// </summary>
    public FallbackHandler? FallbackHandler { get; set; }
}
