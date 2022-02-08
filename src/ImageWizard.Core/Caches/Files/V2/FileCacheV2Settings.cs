using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageWizard.Caches
{
    /// <summary>
    /// FileCacheV2Settings
    /// </summary>
    public class FileCacheV2Settings
    {
        public FileCacheV2Settings()
        {
            Folder = "FileCache";
        }

        /// <summary>
        /// RootFolder
        /// </summary>
        public string Folder { get; set; }
    }
}
