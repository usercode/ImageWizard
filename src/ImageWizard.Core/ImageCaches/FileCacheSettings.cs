using Microsoft.AspNetCore.Hosting;
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
        }

        /// <summary>
        /// RootFolder
        /// </summary>
        public string RootFolder { get; set; }
    }
}
