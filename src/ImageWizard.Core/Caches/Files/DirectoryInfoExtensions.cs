// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

public static class DirectoryInfoExtensions
{
    public static bool IsEmpty(this DirectoryInfo folder)
    {
        return Directory.EnumerateFileSystemEntries(folder.FullName).Any() == false;
    }
}
