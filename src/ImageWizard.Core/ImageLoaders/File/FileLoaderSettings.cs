﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders.File
{
    public class FileLoaderSettings
    {
        public FileLoaderSettings()
        {
            Folder = "FileStorage";
        }

        public string Folder { get; set; }
    }
}
