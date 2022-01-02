using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
{
    public class YouTubeOptions : DataLoaderOptions
    {
        public YouTubeOptions()
        {
            RefreshMode = DataLoaderRefreshMode.BasedOnCacheControl;
        }
    }
}
