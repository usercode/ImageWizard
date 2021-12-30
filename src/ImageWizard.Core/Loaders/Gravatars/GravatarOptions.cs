using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders.Gravatars
{
    public class GravatarOptions : DataLoaderOptions
    {
        public GravatarOptions()
        {
            RefreshMode = DataLoaderRefreshMode.UseRemoteCacheControl;
        }
    }
}
