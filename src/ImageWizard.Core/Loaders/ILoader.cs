// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Options;

namespace ImageWizard.Loaders;

/// <summary>
/// ILoader
/// </summary>
public interface ILoader
{
    /// <summary>
    /// Options
    /// </summary>
    IOptions<LoaderOptions> Options { get; }

    /// <summary>
    /// GetAsync
    /// </summary>
    Task<LoaderResult> GetAsync(string source, CachedData? existingCachedData = null);
}
