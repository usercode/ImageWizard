// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Core.Loaders;

/// <summary>
/// LoaderCacheKey
/// </summary>
public class LoaderCacheKey : ILoaderCacheKey
{
    public string Create(string loaderType, string loaderSource)
    {
        string input = $"{loaderType}/{loaderSource}";

        return input.CreateSHA256Hash();
    }
}
