// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Loaders;

public class GravatarOptions : LoaderOptions
{
    public GravatarOptions()
    {
        RefreshMode = LoaderRefreshMode.BasedOnCacheControl;
        CacheControlMaxAge = TimeSpan.FromDays(7);
    }
}
