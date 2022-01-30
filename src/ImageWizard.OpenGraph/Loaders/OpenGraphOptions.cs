using ImageWizard.Loaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
{
    public class OpenGraphOptions : DataLoaderOptions
    {
        public OpenGraphOptions()
        {
            RefreshMode = DataLoaderRefreshMode.BasedOnCacheControl;
        }
    }
}
