using ImageWizard.Core.Types;
using ImageWizard.ImageStorages;
using ImageWizard.Services.Types;
using ImageWizard.Types;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageCaches
{
    /// <summary>
    /// DistributedCache
    /// </summary>
    public class DistributedCache : IImageCache
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

        public async Task<ICachedImage?> ReadAsync(string key)
        {
            string json = await Cache.GetStringAsync($"{KeyPrefix}{key}#meta");

            if (json == null)
            {
                return null;
            }

            ImageMetadata? metadata = JsonSerializer.Deserialize<ImageMetadata>(json);

            if(metadata == null)
            {
                throw new Exception("Metadata is not available.");
            }

            return new CachedImage(metadata, async () =>
            {
                byte[] b = await Cache.GetAsync($"{KeyPrefix}{key}");

                return new MemoryStream(b);
            });
        }

        public async Task WriteAsync(string key, ICachedImage cachedImage)
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
