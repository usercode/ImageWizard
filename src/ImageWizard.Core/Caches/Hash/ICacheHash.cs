// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Caches;

/// <summary>
/// ICacheHash
/// </summary>
public interface ICacheHash
{
    /// <summary>
    /// Create
    /// </summary>
    Task<string> CreateAsync(Stream stream);
}
