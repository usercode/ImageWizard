﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders.Youtube
{
    public class YouTubeOptions : DataLoaderOptions
    {
        public YouTubeOptions()
        {
            RefreshMode = DataLoaderRefreshMode.UseRemoteCacheControl;
        }
    }
}
