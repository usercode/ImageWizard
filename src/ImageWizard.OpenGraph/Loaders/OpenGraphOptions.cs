// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
{
    public class OpenGraphOptions : LoaderOptions
    {
        public OpenGraphOptions()
        {
            RefreshMode = LoaderRefreshMode.BasedOnCacheControl;
        }
    }
}
