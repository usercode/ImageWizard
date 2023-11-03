// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Security.Cryptography;

namespace ImageWizard.Caches;

public class SHA256CacheHash : ICacheHash
{
    public async Task<string> CreateAsync(Stream stream)
    {
        byte[] hash = await SHA256.HashDataAsync(stream);

        return Convert.ToHexString(hash);
    }
}
