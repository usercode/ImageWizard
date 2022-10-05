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
    /// <param name="key"></param>
    /// <returns></returns>
    Task<ICachedData?> ReadAsync(string key);

    /// <summary>
    /// WriteAsync
    /// </summary>
    /// <param name="metadata"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    Task WriteAsync(IMetadata metadata, Stream stream);
}
