using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders.Files
{
    public class FileLoaderOptions : DataLoaderOptions
    {
        public FileLoaderOptions()
        {
            RefreshMode = DataLoaderRefreshMode.EveryTime;
            Folder = "FileStorage";
        }

        public string Folder { get; set; }
    }
}
