using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
    /// <summary>
    /// OneTimeCache
    /// </summary>
    class OneTimeCache : ICache
    {
        public OneTimeCache()
        {
            _data = Array.Empty<byte>();
        }

        private string? _key;
        private IMetadata? _metadata;
        private byte[] _data;

        public async Task<ICachedData?> ReadAsync(string key)
        {
            if (_key == null)
            {
                return null;
            }

            if (_key != key)
            {
                throw new Exception("Internal cache error.");
            }

            return new CachedData(_metadata, () => Task.FromResult<Stream>(new MemoryStream(_data)));
        }

        public async Task WriteAsync(string key, ICachedData cachedData)
        {
            _key = key;
            _metadata = cachedData.Metadata;

            MemoryStream mem = new MemoryStream();

            using Stream dataStream = await cachedData.OpenReadAsync();

            await dataStream.CopyToAsync(mem);

            _data = mem.ToArray();
        }
    }
}
