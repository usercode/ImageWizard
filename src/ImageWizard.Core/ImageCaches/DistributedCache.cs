using ImageWizard.Core.Types;
using ImageWizard.ImageStorages;
using ImageWizard.Services.Types;
using ImageWizard.Types;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public async Task<ICachedImage> ReadAsync(string key)
        {
            string json = await Cache.GetStringAsync($"{key}#meta");

            if (json == null)
            {
                return null;
            }

            IImageMetadata metadata = JsonConvert.DeserializeObject<ImageMetadata>(json);

            return new CachedImage(metadata, async () =>
            {
                byte[] b = await Cache.GetAsync(key);

                return new MemoryStream(b);
            });
        }

        public async Task WriteAsync(string key, IImageMetadata metadata, byte[] buffer)
        {
            string json = JsonConvert.SerializeObject(metadata);

            await Cache.SetStringAsync($"{key}#meta", json);

            await Cache.SetAsync(key, buffer);
        }
    }
}
