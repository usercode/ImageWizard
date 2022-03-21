// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
