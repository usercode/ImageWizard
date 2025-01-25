// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Caches;

/// <summary>
/// ICache
/// </summary>
public interface ICache
{
    /// <summary>
    /// ReadAsync
    /// </summary>
    Task<CachedData?> ReadAsync(string key);

    /// <summary>
    /// WriteAsync
    /// </summary>
    Task WriteAsync(Metadata metadata, Stream stream);
}
