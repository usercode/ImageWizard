using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
{
    public class GravatarOptions : LoaderOptions
    {
        public GravatarOptions()
        {
            RefreshMode = LoaderRefreshMode.BasedOnCacheControl;
        }
    }
}
