using ImageWizard.Core.Types;
using ImageWizard.Metadatas;
using ImageWizard.Services.Types;
using ImageWizard.Types;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
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

        private const string KeyPrefix = "ImageWizard:";

        public async Task<ICachedData?> ReadAsync(string key)
        {
            string json = await Cache.GetStringAsync($"{KeyPrefix}{key}#meta");

            if (json == null)
            {
                return null;
            }

            Metadata? metadata = JsonSerializer.Deserialize<Metadata>(json);

            if(metadata == null)
            {
                throw new Exception("Metadata is not available.");
            }

            return new CachedData(metadata, async () =>
            {
                byte[] b = await Cache.GetAsync($"{KeyPrefix}{key}");

                return new MemoryStream(b);
            });
        }

        public async Task WriteAsync(string key, ICachedData cachedImage)
        {
            string json = JsonSerializer.Serialize(cachedImage.Metadata);

            await Cache.SetStringAsync($"{KeyPrefix}{key}#meta", json);

            using (Stream cachedImageStream = await cachedImage.OpenReadAsync())
            {
                MemoryStream mem = new MemoryStream();
                await cachedImageStream.CopyToAsync(mem);

                await Cache.SetAsync($"{KeyPrefix}{key}", mem.ToArray());
            }
        }
    }
}
