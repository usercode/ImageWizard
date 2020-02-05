using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders
{
    public class ImageLoaderOptions
    {
        public ImageLoaderOptions()
        {
            RefreshMode = ImageLoaderRefreshMode.None;
        }

        public ImageLoaderRefreshMode RefreshMode { get; set; }
    }
}
