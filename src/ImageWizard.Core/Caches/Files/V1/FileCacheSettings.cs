// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageWizard.Caches;

/// <summary>
/// FileCacheSettings
/// </summary>
public class FileCacheSettings
{
    public FileCacheSettings()
    {
        Folder = "FileCache";
    }

    /// <summary>
    /// RootFolder
    /// </summary>
    public string Folder { get; set; }
}
