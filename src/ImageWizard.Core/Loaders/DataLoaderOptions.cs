using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders
{
    public class DataLoaderOptions
    {
        public DataLoaderOptions()
        {
            RefreshMode = DataLoaderRefreshMode.None;
        }

        public DataLoaderRefreshMode RefreshMode { get; set; }
    }
}
