// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Core.Loaders;

/// <summary>
/// ILoaderCacheKey
/// </summary>
public interface ILoaderCacheKey
{
    /// <summary>
    /// Create
    /// </summary>
    string Create(string loaderType, string loaderSource);
}
