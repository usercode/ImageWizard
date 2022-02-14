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
