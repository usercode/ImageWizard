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
        public FileCacheSettings(string rootFolder)
            : this(new DirectoryInfo(rootFolder))
        {

        }

         public FileCacheSettings(DirectoryInfo rootFolder)
        { 
            RootFolder = rootFolder;

            if(RootFolder.Exists == false)
            {
                throw new Exception("Cache folder doesn't exists: " + RootFolder.Name);
            }
        }

        /// <summary>
        /// RootFolder
        /// </summary>
        public DirectoryInfo RootFolder { get; }
    }
}
