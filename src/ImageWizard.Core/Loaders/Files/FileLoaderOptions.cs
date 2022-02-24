// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
{
    public class FileLoaderOptions : LoaderOptions
    {
        public FileLoaderOptions()
        {
            RefreshMode = LoaderRefreshMode.EveryTime;
            Folder = "FileStorage";
        }

        public string Folder { get; set; }
    }
}
