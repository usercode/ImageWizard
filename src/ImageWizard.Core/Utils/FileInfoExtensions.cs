// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

static class FileInfoExtensions
{
    public static string GetEtag(this IFileInfo fileInfo)
    {
        string etag = (fileInfo.Length ^ fileInfo.LastModified.UtcTicks).ToString();

        return etag;
    }

    public static string GetEtag(this FileInfo fileInfo)
    {
        string etag = (fileInfo.Length ^ fileInfo.LastWriteTimeUtc.Ticks).ToString();

        return etag;
    }    
}
