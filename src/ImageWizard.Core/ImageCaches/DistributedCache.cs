using ImageWizard.Core.Types;
using ImageWizard.ImageStorages;
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
            Serializer = new CachedFileSerializer();
        }

        /// <summary>
        /// Cache
        /// </summary>
        public IDistributedCache Cache { get; }

        /// <summary>
        /// Serializer
        /// </summary>
        private CachedFileSerializer Serializer { get; }

        public async Task<CachedImage> ReadAsync(string key)
        {
            byte[] buffer = await Cache.GetAsync(key);

            if(buffer == null)
            {
                return null;
            }

            CachedImage cachedImage = Serializer.Read(buffer);

            return cachedImage;
        }

        public async Task WriteAsync(string key, CachedImage cachedImage)
        {
            byte[] buffer = Serializer.Write(cachedImage);

            await Cache.SetAsync(key, buffer);
        }
    }
}
