using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
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
