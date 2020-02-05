using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders.Files
{
    public class FileLoaderOptions : ImageLoaderOptions
    {
        public FileLoaderOptions()
        {
            RefreshMode = ImageLoaderRefreshMode.EveryTime;
            Folder = "FileStorage";
        }

        public string Folder { get; set; }
    }
}
