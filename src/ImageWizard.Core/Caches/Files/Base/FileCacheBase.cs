// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

/// <summary>
/// FileCacheBase
/// </summary>
public abstract class FileCacheBase<TOptions> : ICache, ICleanupCache, ILastAccessCache
    where TOptions : FileCacheOptionsBase
{
    protected static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };

    public FileCacheBase(IOptions<TOptions> settings, IWebHostEnvironment hostingEnvironment)
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
    public IOptions<TOptions> Settings { get; }

    /// <summary>
    /// Folder
    /// </summary>
    public DirectoryInfo Folder { get; }

    protected virtual DirectoryInfo GetMetaFolder() => new DirectoryInfo(Folder.FullName);

    protected abstract FileInfo GetFile(FileType type, string key);

    protected abstract string GetBlobField(IMetadata metadata);

    public virtual async Task<ICachedData?> ReadAsync(string key)
    {
        Metadata? metadata = await ReadMetadataAsync(key);

        if (metadata == null)
        {
            return null;
        }

        FileInfo blobFile = GetFile(FileType.Blob, GetBlobField(metadata));

        if (blobFile.Exists == false)
        {
            return null;
        }

        return new CachedData(metadata, () => Task.FromResult<Stream>(blobFile.OpenRead()));
    }

    public virtual async Task WriteAsync(string key, IMetadata metadata, Stream stream)
    {
        await WriteMetadataAsync(metadata);

        FileInfo blobFile = GetFile(FileType.Blob, GetBlobField(metadata));

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

    private Task<Metadata?> ReadMetadataAsync(string key)
    {
        return ReadMetadataAsync(GetFile(FileType.Meta, key));
    }

    private async Task<Metadata?> ReadMetadataAsync(FileInfo metaFile)
    {
        if (metaFile.Exists == false)
        {
            return null;
        }

        using Stream metadataStream = metaFile.OpenRead();

        Metadata? metadata = await JsonSerializer.DeserializeAsync<Metadata>(metadataStream);

        return metadata;
    }

    private async Task WriteMetadataAsync(IMetadata metadata)
    {
        FileInfo metaFile = GetFile(FileType.Meta, metadata.Key);

        if (metaFile.Directory != null)
        {
            //create folder structure for meta file
            metaFile.Directory.Create();
        }

        using Stream metadataStream = metaFile.OpenWrite();

        //delete existing data
        metadataStream.SetLength(0);

        await JsonSerializer.SerializeAsync(metadataStream, metadata, JsonSerializerOptions);
    }

    private void Delete(IMetadata metadata)
    {
        FileInfo metaFile = GetFile(FileType.Meta, metadata.Key);
        FileInfo blobFile = GetFile(FileType.Blob, GetBlobField(metadata));

        if (metaFile.Exists)
        {
            metaFile.Delete();
        }

        if (blobFile.Exists)
        {
            blobFile.Delete();
        }
        
        DeleteEmptyFolder(metaFile.Directory);
        DeleteEmptyFolder(blobFile.Directory);
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

    public virtual async Task CleanupAsync(IEnumerable<CleanupReason> reasons, CancellationToken cancellationToken = default)
    {
        DirectoryInfo metaFolder = GetMetaFolder();

        if (metaFolder.Exists == false)
        {
            return;
        }

        foreach (DirectoryInfo level1 in metaFolder.GetDirectories())
        {
            foreach (DirectoryInfo level2 in level1.GetDirectories())
            {
                foreach (DirectoryInfo level3 in level2.GetDirectories())
                {
                    foreach (DirectoryInfo level4 in level3.GetDirectories())
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        foreach (FileInfo metaFile in level4.GetFiles($"*.{FileType.Meta.ToTypeString()}"))
                        {
                            IMetadata? metadata = await ReadMetadataAsync(metaFile);

                            if (metadata != null)
                            {
                                bool invalid = reasons.Any(r => r.IsValid(metadata) == true);

                                if (invalid)
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
