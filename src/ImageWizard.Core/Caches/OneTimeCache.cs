// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Caches;

/// <summary>
/// OneTimeCache
/// </summary>
class OneTimeCache : ICache
{
    private Metadata? _metadata;
    private byte[] _data = [];

    public async Task<CachedData?> ReadAsync(string key)
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

    public async Task WriteAsync(Metadata metadata, Stream stream)
    {
        _metadata = metadata;

        MemoryStream mem = new MemoryStream();

        await stream.CopyToAsync(mem);

        _data = mem.ToArray();
    }
}
