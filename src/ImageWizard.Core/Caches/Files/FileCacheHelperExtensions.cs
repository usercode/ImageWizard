// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Caches;

public static class FileCacheHelperExtensions
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

    public static bool IsEmpty(this DirectoryInfo folder)
    {
        return Directory.EnumerateFileSystemEntries(folder.FullName).Any() == false;
    }
}
