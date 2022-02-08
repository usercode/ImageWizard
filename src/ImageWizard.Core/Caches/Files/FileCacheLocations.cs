using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
    /// <summary>
    /// FileCacheLocations
    /// </summary>
    public struct FileCacheLocations
    {
        public FileCacheLocations(FileInfo metafile, FileInfo blobfile)
        {
            MetaFile = metafile;
            BlobFile = blobfile;
        }

        /// <summary>
        /// MetaFile
        /// </summary>
        public FileInfo MetaFile { get; }

        /// <summary>
        /// BlobFile
        /// </summary>
        public FileInfo BlobFile { get; }
    }
}
