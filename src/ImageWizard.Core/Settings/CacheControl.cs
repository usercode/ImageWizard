// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

/// <summary>
/// CacheControl
/// </summary>
public class CacheControl
{
    /// <summary>
    /// IsEnabled
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Public
    /// </summary>
    public bool Public { get; set; } = true;

    /// <summary>
    /// ResponseCacheTime
    /// </summary>
    public TimeSpan? MaxAge { get; set; } = TimeSpan.FromDays(365);

    /// <summary>
    /// MustRevalidate
    /// </summary>
    public bool MustRevalidate { get; set; } = false;

    /// <summary>
    /// NoCache
    /// </summary>
    public bool NoCache { get; set; } = false;

    /// <summary>
    /// NoStore
    /// </summary>
    public bool NoStore { get; set; } = false;
}
