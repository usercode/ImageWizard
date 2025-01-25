// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Options;

namespace ImageWizard.Loaders;

/// <summary>
/// Loader
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public abstract class Loader<TOptions> : ILoader
    where TOptions : LoaderOptions
{
    public Loader(IOptions<TOptions> options)
    {
        Options = options;
    }

    /// <summary>
    /// Options
    /// </summary>
    public IOptions<TOptions> Options { get; }

    IOptions<LoaderOptions> ILoader.Options => Options;

    public abstract Task<LoaderResult> GetAsync(string source, CachedData? existingCachedData);
}
