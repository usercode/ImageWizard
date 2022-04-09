// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;
using ImageWizard.Core;
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
/// FileCache with data deduplication
/// </summary>
public class FileCacheV2 : FileCacheBase<FileCacheV2Options>
{
    public FileCacheV2(IOptions<FileCacheV2Options> options, IWebHostEnvironment hostingEnvironment, ICacheLock cacheLock)
        : base(options, hostingEnvironment, cacheLock)
    {
    }

    protected override DirectoryInfo GetMetaFolder() => new DirectoryInfo(Path.Join(Folder.FullName, FileType.Meta.ToTypeString()));

    protected override string GetBlobField(IMetadata metadata) => metadata.Hash;

    protected override FileInfo GetFile(FileType type, string key)
    {
        string typeString = type.ToTypeString();

        string folders = Path.Join(
                                key.AsSpan(0, 2),
                                key.AsSpan(2, 2),
                                key.AsSpan(4, 2),
                                key.AsSpan(6, 2));

        ReadOnlySpan<char> filename = key.AsSpan(8);

        string file = Path.Join(Folder.FullName, typeString, folders, $"{filename}.{typeString}");

        return new FileInfo(file);
    }
}
