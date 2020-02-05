using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders.Youtube
{
    public class YouTubeOptions : ImageLoaderOptions
    {
        public YouTubeOptions()
        {
            RefreshMode = ImageLoaderRefreshMode.UseRemoteCacheControl;
        }
    }
}
