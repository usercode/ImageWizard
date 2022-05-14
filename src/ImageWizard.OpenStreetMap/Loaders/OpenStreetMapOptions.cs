// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.OpenStreetMap;

public class OpenStreetMapOptions : LoaderOptions
{
    public OpenStreetMapOptions()
    {
        RefreshMode = LoaderRefreshMode.BasedOnCacheControl;
        CacheControlMaxAge = TimeSpan.FromDays(7);
        Path = "https://a.tile.openstreetmap.org";
    }

    public string Path { get; set; }
}