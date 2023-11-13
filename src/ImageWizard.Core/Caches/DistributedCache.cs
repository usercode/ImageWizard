// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Json;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ImageWizard.Caches;

/// <summary>
/// DistributedCache
/// </summary>
public class DistributedCache : ICache
{
    public DistributedCache(IDistributedCache distributedCache)
    {
        Cache = distributedCache;
    }

    /// <summary>
    /// Cache
    /// </summary>
    public IDistributedCache Cache { get; }

    private const string KeyPrefix = "ImageWizard";

    private static string GetBlobKey(string key)
    {
        return $"{KeyPrefix}_{key}_blob";
    }

    private static string GetMetaKey(string key)
    {
        return $"{KeyPrefix}_{key}_meta";
    }

    public async Task<ICachedData?> ReadAsync(string key)
    {
        byte[]? json = await Cache.GetAsync(GetMetaKey(key));

        if (json == null)
        {
            return null;
        }

        Metadata? metadata = JsonSerializer.Deserialize(json, ImageWizardJsonSerializerContext.Default.Metadata);

        if (metadata == null)
        {
            return null;
        }

        return new CachedData(metadata, async () =>
        {
            byte[]? b = await Cache.GetAsync(GetBlobKey(key));

            if (b == null)
            {
                throw new Exception();
            }

            return new MemoryStream(b);
        });
    }

    public async Task WriteAsync(IMetadata metadata, Stream stream)
    {
        byte[] json = JsonSerializer.SerializeToUtf8Bytes(metadata, ImageWizardJsonSerializerContext.Default.Metadata);

        await Cache.SetAsync(GetMetaKey(metadata.Key), json);

        using MemoryStream mem = new MemoryStream();

        await stream.CopyToAsync(mem);

        await Cache.SetAsync(GetBlobKey(metadata.Key), mem.ToArray());
    }
}
