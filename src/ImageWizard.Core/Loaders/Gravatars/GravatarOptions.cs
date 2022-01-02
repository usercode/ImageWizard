using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
{
    public class GravatarOptions : DataLoaderOptions
    {
        public GravatarOptions()
        {
            RefreshMode = DataLoaderRefreshMode.BasedOnCacheControl;
        }
    }
}
