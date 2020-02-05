using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders.Gravatars
{
    public class GravatarOptions : ImageLoaderOptions
    {
        public GravatarOptions()
        {
            RefreshMode = ImageLoaderRefreshMode.UseRemoteCacheControl;
        }
    }
}
