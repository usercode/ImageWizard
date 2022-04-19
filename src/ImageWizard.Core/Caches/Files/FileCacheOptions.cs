// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageWizard.Caches;

/// <summary>
/// FileCacheOptions
/// </summary>
public class FileCacheOptions
{
    public FileCacheOptions()
    {
        Folder = "FileCache";
    }

    /// <summary>
    /// RootFolder
    /// </summary>
    public string Folder { get; set; }
}
