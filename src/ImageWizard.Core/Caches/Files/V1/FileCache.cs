// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

/// <summary>
/// FileCache
/// </summary>
public class FileCache : ICache
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };

    public FileCache(IOptions<FileCacheSettings> settings, IWebHostEnvironment hostingEnvironment)
    {
        Settings = settings;

        if (Path.IsPathFullyQualified(settings.Value.Folder))
        {
            Folder = new DirectoryInfo(settings.Value.Folder);
        }
        else
        {
            Folder = new DirectoryInfo(Path.Join(hostingEnvironment.ContentRootPath, settings.Value.Folder));
        }
    }

    /// <summary>
    /// Settings
    /// </summary>
    public IOptions<FileCacheSettings> Settings { get; }

    /// <summary>
    /// Folder
    /// </summary>
    public DirectoryInfo Folder { get; }

    protected (FileInfo metafile, FileInfo blobfile) GetFileCacheLocations(string key)
    {
        string folders = Path.Join(
                                key.AsSpan(0, 2),
                                key.AsSpan(2, 2),
                                key.AsSpan(4, 2),
                                key.AsSpan(6, 2));

        ReadOnlySpan<char> filename = key.AsSpan(8);

        string basePath = Path.Join(Folder.FullName, folders, filename);

        return (new FileInfo($"{basePath}.meta"), new FileInfo($"{basePath}.blob"));
    }

    public async Task<ICachedData?> ReadAsync(string key)
    {
        var (metafile, blobfile) = GetFileCacheLocations(key);

        if (metafile.Exists == false || blobfile.Exists == false)
        {
            return null;
        }

        using Stream metadataStream = metafile.OpenRead();

        Metadata? metadata = await JsonSerializer.DeserializeAsync<Metadata>(metadataStream);

        if (metadata == null)
        {
            throw new ArgumentNullException(nameof(metadata));
        }

        return new CachedData(metadata, () => Task.FromResult<Stream>(blobfile.OpenRead()));
    }

    public async Task WriteAsync(string key, IMetadata metadata, Stream stream)
    {
        var (metafile, blobfile) = GetFileCacheLocations(key);

        if (metafile.Directory != null)
        {
            //create folder structure for meta and blob file
            metafile.Directory.Create();
        }

        using Stream metadataStream = metafile.OpenWrite();

        //delete existing data
        metadataStream.SetLength(0);

        await JsonSerializer.SerializeAsync(metadataStream, metadata, JsonSerializerOptions);

        //write data
        using Stream blobStream = blobfile.OpenWrite();

        //delete existing data
        blobStream.SetLength(0);

        await stream.CopyToAsync(blobStream);
    }
}
