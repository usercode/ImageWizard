// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Caches;

public class SHA256CacheKey : ICacheKey
{
    public string Create(string input)
    {
        return input.CreateSHA256Hash();
    }
}
