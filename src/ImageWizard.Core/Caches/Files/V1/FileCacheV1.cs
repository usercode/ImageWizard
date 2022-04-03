// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageWizard.Caches;

/// <summary>
/// FileCacheV1
/// </summary>
public class FileCacheV1 : FileCacheBase<FileCacheOptionsBase>
{
    public FileCacheV1(IOptions<FileCacheV1Options> options, IWebHostEnvironment hostingEnvironment)
        : base(options, hostingEnvironment)
    {

    }

    protected override string GetBlobField(IMetadata metadata) => metadata.Key;

    protected override FileInfo GetFile(FileType type, string key)
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
}
