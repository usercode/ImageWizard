using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
{
    public class YouTubeOptions : LoaderOptions
    {
        public YouTubeOptions()
        {
            RefreshMode = LoaderRefreshMode.BasedOnCacheControl;
        }
    }
}
