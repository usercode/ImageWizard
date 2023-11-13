// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard.Client;

/// <summary>
/// DeliveryTypeExtensions
/// </summary>
public static class DeliveryTypeExtensions
{
    /// <summary>
    /// Fetch file from absolute or relative url.
    /// </summary>
    public static IFilter Fetch(this ILoader imageUrlBuilder, string url)
    {
        return imageUrlBuilder.LoadData("fetch", url);
    }

    /// <summary>
    /// Fetch file from wwwroot folder with fingerprint.
    /// </summary>
    public static IFilter FetchLocalFile(this ILoader imageBuilder, string path, int maxVersionLength = 8)
    {
        path = path.TrimStart('/');

        IWebHostEnvironment env = imageBuilder.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        IFileInfo file = env.WebRootFileProvider.GetFileInfo(path);

        string hash = $"{file.Length}_{file.LastModified.UtcTicks}";

        Span<byte> hashBufferSpan = stackalloc byte[32];

        SHA256.HashData(Encoding.UTF8.GetBytes(hash), hashBufferSpan);

        string hashBase64 = WebEncoders.Base64UrlEncode(hashBufferSpan);

        path += $"?v={hashBase64.AsSpan(0, maxVersionLength)}";

        return imageBuilder.LoadData("fetch", path);
    }

    public static IFilter Azure(this ILoader imageUrlBuilder, string url)
    {
        return imageUrlBuilder.LoadData("azure", url);
    }

    public static IFilter AWS(this ILoader imageUrlBuilder, string url)
    {
        return imageUrlBuilder.LoadData("aws", url);
    }

    public static IFilter File(this ILoader imageUrlBuilder, string path)
    {
        return imageUrlBuilder.LoadData("file", path);
    }
}
