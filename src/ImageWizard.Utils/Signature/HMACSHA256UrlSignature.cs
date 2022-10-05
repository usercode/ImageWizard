// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard.Utils;

/// <summary>
/// HMACSHA256UrlSignature
/// </summary>
public class HMACSHA256UrlSignature : IUrlSignature
{
    public HMACSHA256UrlSignature()
    {
        IncludeHost = false;
    }

    public HMACSHA256UrlSignature(bool includeHost)
    {
        IncludeHost = includeHost;
    }

    private IMemoryCache _cache = new MemoryCache(Options.Create(new MemoryCacheOptions() { SizeLimit = 10_000 }));

    /// <summary>
    /// Signature depend on remote hostname? (Default: false)
    /// </summary>
    public virtual bool IncludeHost { get; }

    /// <summary>
    /// Selects part of the url.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    protected virtual string GetUrlValue(ImageWizardUrl url)
    {
        return url.Path;
    }

    /// <summary>
    /// Selects part of the host.
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    protected virtual string GetHostValue(HostString host)
    {
        return host.Value;
    }

    /// <summary>
    /// Encrypt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string Encrypt(byte[] key, ImageWizardRequest request)
    {
        //build data string
        string input;

        if (IncludeHost == true)
        {
            input = $"{GetHostValue(request.Host)}/{GetUrlValue(request.Url)}";
        }
        else
        {
            input = GetUrlValue(request.Url);
        }

        //signature already exists in cache?
        if (_cache.TryGetValue(input, out string? cachedKey) == true)
        {
            if (cachedKey != null)
            {
                return cachedKey;
            }
        }

        int inputLength = Encoding.UTF8.GetByteCount(input);

        Span<byte> inputBuffer = inputLength <= 128 ? stackalloc byte[inputLength] : new byte[inputLength];

        Encoding.UTF8.GetBytes(input, inputBuffer);

        //HMACSHA256 => 256 / 8 = 32
        Span<byte> hashBuffer = stackalloc byte[32];

        //create hash
        HMACSHA256.HashData(key, inputBuffer, hashBuffer);

        //convert to Base64Url
        string keyBase64Url = WebEncoders.Base64UrlEncode(hashBuffer);

        //add signature to cache
        _cache.Set(input, keyBase64Url, new MemoryCacheEntryOptions() 
                                            { 
                                                Size = 1,
                                                SlidingExpiration = TimeSpan.FromHours(1)
                                            });

        return keyBase64Url;
    }
}
