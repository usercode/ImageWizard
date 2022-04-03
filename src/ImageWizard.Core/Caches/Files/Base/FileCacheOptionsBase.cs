// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageWizard.Caches;

/// <summary>
/// FileCacheOptionsBase
/// </summary>
public abstract class FileCacheOptionsBase
{
    public FileCacheOptionsBase()
    {
        Folder = "FileCache";
    }

    /// <summary>
    /// RootFolder
    /// </summary>
    public string Folder { get; set; }
}
