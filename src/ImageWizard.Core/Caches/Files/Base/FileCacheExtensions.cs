// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

public static class FileCacheExtensions
{
    public static string ToTypeString(this FileType type)
    {
        return type switch
        {
            FileType.Meta => "meta",
            FileType.Blob => "blob",
            _ => throw new Exception()
        };
    }
}
