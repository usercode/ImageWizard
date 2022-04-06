// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

public class SHA256CacheKey : ICacheKey
{
    public string Create(string input)
    {
        int inputLength = Encoding.UTF8.GetByteCount(input);

        Span<byte> inputBuffer = inputLength <= 128 ? stackalloc byte[inputLength] : new byte[inputLength];

        Encoding.UTF8.GetBytes(input, inputBuffer);

        Span<byte> hashBuffer = stackalloc byte[32];

        SHA256.HashData(inputBuffer, hashBuffer);

        string key = Convert.ToHexString(hashBuffer);

        return key;
    }
}
