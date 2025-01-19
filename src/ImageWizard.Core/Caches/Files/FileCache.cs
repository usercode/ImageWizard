// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;
using ImageWizard.Core.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ImageWizard.Caches;

/// <summary>
/// FileCache
/// </summary>
public class FileCache : ICache, ICleanupCache, ILastAccessCache
{
    public FileCache(
                    IOptions<FileCacheOptions> options, 
                    IWebHostEnvironment hostingEnvironment, 
                    ICacheLock cacheLock,
                    ILogger<FileCache> logger)
    {
        Options = options;
        CacheLock = cacheLock;
        Logger = logger;

        if (Path.IsPathFullyQualified(options.Value.Folder))
        {
            Folder = new DirectoryInfo(options.Value.Folder);
        }
        else
        {
            Folder = new DirectoryInfo(Path.Join(hostingEnvironment.ContentRootPath, options.Value.Folder));
        }
    }

    /// <summary>
    /// Options
    /// </summary>
    public IOptions<FileCacheOptions> Options { get; }

    /// <summary>
    /// Folder
    /// </summary>
    public DirectoryInfo Folder { get; }

    /// <summary>
    /// CacheLock
    /// </summary>
    private ICacheLock CacheLock { get; }

    /// <summary>
    /// Logger
    /// </summary>
    private ILogger<FileCache> Logger { get; }

    protected FileInfo GetFileInfo(FileType type, string key)
    {
        string typeString = type.ToTypeString();

        string folders = Path.Join(
                                key.AsSpan(0, 2),
                                key.AsSpan(2, 2),
                                key.AsSpan(4, 2),
                                key.AsSpan(6, 2));

        ReadOnlySpan<char> filename = key.AsSpan(8);

        string file = Path.Join(Folder.FullName, folders, $"{filename}.{typeString}");

        return new FileInfo(file);
    }

    public virtual async Task<ICachedData?> ReadAsync(string key)
    {
        Metadata? metadata = await ReadMetadataAsync(key);

        if (metadata == null)
        {
            return null;
        }

        FileInfo blobFile = GetFileInfo(FileType.Blob, metadata.Key);

        if (blobFile.Exists == false)
        {
            return null;
        }

        return new CachedData(metadata, () => Task.FromResult<Stream>(blobFile.OpenRead()));
    }

    public virtual async Task WriteAsync(IMetadata metadata, Stream stream)
    {
        await WriteMetadataAsync(metadata);

        FileInfo blobFile = GetFileInfo(FileType.Blob, metadata.Key);

        if (blobFile.Directory != null)
        {
            //create folder structure for blob file
            blobFile.Directory.Create();
        }

        //write data
        using Stream blobStream = blobFile.OpenWrite();

        //delete existing data
        blobStream.SetLength(0);

        await stream.CopyToAsync(blobStream);
    }

    private async Task<Metadata?> ReadMetadataAsync(string key)
    {
        FileInfo metaFile = GetFileInfo(FileType.Meta, key);

        if (metaFile.Exists == false)
        {
            return null;
        }

        using Stream metadataStream = metaFile.OpenRead();

        try
        {
            Metadata? metadata = await JsonSerializer.DeserializeAsync(metadataStream, ImageWizardJsonSerializerContext.Default.Metadata);

            return metadata;
        }
        catch(Exception ex)
        {
            Logger.LogWarning(ex, $"Reading metadata is failed for key \"{key}\"");

            return null; //simulate "not found".to recreate a new cached data.
        }
    }

    private async Task WriteMetadataAsync(IMetadata metadata)
    {
        FileInfo metaFile = GetFileInfo(FileType.Meta, metadata.Key);

        if (metaFile.Directory != null)
        {
            //create folder structure for meta file
            metaFile.Directory.Create();
        }

        using Stream metadataStream = metaFile.OpenWrite();

        //delete existing data
        metadataStream.SetLength(0);

        await JsonSerializer.SerializeAsync(metadataStream, metadata, ImageWizardJsonSerializerContext.Default.Metadata);
    }

    private void Delete(IMetadata metadata)
    {
        FileInfo metaFile = GetFileInfo(FileType.Meta, metadata.Key);
        FileInfo blobFile = GetFileInfo(FileType.Blob, metadata.Key);

        if (metaFile.Exists)
        {
            metaFile.Delete();
        }

        if (blobFile.Exists)
        {
            blobFile.Delete();
        }
        
        DeleteEmptyFolder(metaFile.Directory);
        //DeleteEmptyFolder(blobFile.Directory);
    }

    private void DeleteEmptyFolder(DirectoryInfo? folder, int level = 1)
    {
        if (level > 4)
        {
            return;
        }

        if (folder == null)
        {
            return;
        }

        if (folder.Exists == false)
        {
            return;
        }

        if (folder.IsEmpty())
        {
            folder.Delete();

            DeleteEmptyFolder(folder.Parent, level + 1);
        }
    }

    public virtual async Task CleanupAsync(CleanupReason reason, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (Folder.Exists == false)
        {
            return;
        }

        foreach (DirectoryInfo level1 in Folder.GetDirectories())
        {
            foreach (DirectoryInfo level2 in level1.GetDirectories())
            {
                foreach (DirectoryInfo level3 in level2.GetDirectories())
                {
                    foreach (DirectoryInfo level4 in level3.GetDirectories())
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        foreach (FileInfo metaFile in level4.GetFiles($"*.{FileType.Meta.ToTypeString()}"))
                        {
                            string key = Path.GetFileNameWithoutExtension($"{level1.Name}{level2.Name}{level3.Name}{level4.Name}{metaFile.Name}");

                            //set lock
                            using var w = await CacheLock.WriterLockAsync(key);

                            //read metadata
                            IMetadata? metadata = await ReadMetadataAsync(key);

                            if (metadata != null)
                            {
                                if (reason.IsValid(metadata))
                                {
                                    Delete(metadata);
                                }
                            }
                        }

                        //little break to prevent high cpu load
                        await Task.Delay(TimeSpan.FromMilliseconds(80), cancellationToken);
                    }
                }
            }
        }
    }

    public async Task SetLastAccessAsync(string key, DateTime dateTime)
    {
        IMetadata? metadata = await ReadMetadataAsync(key);

        if (metadata == null)
        {
            return;
        }

        metadata.LastAccess = dateTime;

        await WriteMetadataAsync(metadata);
    }
}
