using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageWizard.Core.ImageCaches
{
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
}
