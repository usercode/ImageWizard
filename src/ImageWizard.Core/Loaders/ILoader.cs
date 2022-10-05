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
    IOptions<LoaderOptions> Options { get; }

    /// <summary>
    /// GetAsync
    /// </summary>
    /// <param name="requestUri"></param>
    /// <returns></returns>
    Task<LoaderResult> GetAsync(string source, ICachedData? existingCachedData = null);
}
