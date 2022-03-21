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
    protected static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };

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
        var locations = GetFileCacheLocations(key);

        if (locations.metafile.Exists == false || locations.blobfile.Exists == false)
        {
            return null;
        }

        using Stream metadataStream = locations.metafile.OpenRead();

        Metadata? metadata = await JsonSerializer.DeserializeAsync<Metadata>(metadataStream);

        if (metadata == null)
        {
            throw new ArgumentNullException(nameof(metadata));
        }

        return new CachedData(metadata, () => Task.FromResult<Stream>(locations.blobfile.OpenRead()));
    }

    public async Task WriteAsync(string key, IMetadata metadata, Stream stream)
    {
        var locations = GetFileCacheLocations(key);

        if (locations.metafile.Directory != null)
        {
            //create folder structure for meta and blob file
            locations.metafile.Directory.Create();
        }

        using Stream metadataStream = locations.metafile.OpenWrite();

        //delete existing data
        metadataStream.SetLength(0);

        await JsonSerializer.SerializeAsync(metadataStream, metadata, JsonSerializerOptions);

        //write data
        using Stream blobStream = locations.blobfile.OpenWrite();

        //delete existing data
        blobStream.SetLength(0);

        await stream.CopyToAsync(blobStream);
    }
}
