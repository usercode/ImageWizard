// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;

namespace ImageWizard.Loaders;

public class OpenGraphOptions : LoaderOptions
{
    public OpenGraphOptions()
    {
        RefreshMode = LoaderRefreshMode.BasedOnCacheControl;
        CacheControlMaxAge = TimeSpan.FromDays(7);
    }
}
