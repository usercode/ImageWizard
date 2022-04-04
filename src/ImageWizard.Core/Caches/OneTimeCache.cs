// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

/// <summary>
/// OneTimeCache
/// </summary>
class OneTimeCache : ICache
{
    public OneTimeCache()
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

        if (_metadata.Key != key)
        {
            throw new Exception("Internal cache error.");
        }

        return new CachedData(_metadata, () => Task.FromResult<Stream>(new MemoryStream(_data)));
    }

    public async Task WriteAsync(IMetadata metadata, Stream stream)
    {
        _metadata = metadata;

        MemoryStream mem = new MemoryStream();

        await stream.CopyToAsync(mem);

        _data = mem.ToArray();
    }
}
