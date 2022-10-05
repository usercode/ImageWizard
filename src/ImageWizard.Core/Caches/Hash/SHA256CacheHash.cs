// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Security.Cryptography;

namespace ImageWizard.Caches;

public class SHA256CacheHash : ICacheHash
{
    public async Task<string> CreateAsync(Stream stream)
    {
        using SHA256 sha256 = SHA256.Create();

        byte[] hash = await sha256.ComputeHashAsync(stream);

        return Convert.ToHexString(hash);
    }
}
