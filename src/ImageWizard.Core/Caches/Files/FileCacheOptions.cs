// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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
