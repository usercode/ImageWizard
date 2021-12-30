using ImageWizard.Core.StreamPooling;
using ImageWizard.Core.Types;
using ImageWizard.Metadatas;
using ImageWizard.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
    /// <summary>
    /// InMemoryCache
    /// </summary>
    public class InMemoryCache : ICache, IDisposable
    {
        public InMemoryCache()
        {
            _data = Array.Empty<byte>();
        }

        private IMetadata? _metadata;
        private byte[] _data;

        public async Task<ICachedData?> ReadAsync(string key)
        {
            if (_metadata == null)
            {
                return null;
            }

            return new CachedData(_metadata, async () => new MemoryStream(_data));
        }

        public async Task WriteAsync(string key, ICachedData cachedData)
        {
            _metadata = cachedData.Metadata;

            MemoryStream mem = new MemoryStream();

            using Stream dataStream = await cachedData.OpenReadAsync();
            dataStream.CopyTo(mem);

            _data = mem.ToArray();
        }

        public void Dispose()
        {
            
        }
    }
}
