// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
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

    public async Task WriteAsync(string key, IMetadata metadata, Stream stream)
    {
        _key = key;
        _metadata = metadata;

        MemoryStream mem = new MemoryStream();

        await stream.CopyToAsync(mem);

        _data = mem.ToArray();
    }
}
