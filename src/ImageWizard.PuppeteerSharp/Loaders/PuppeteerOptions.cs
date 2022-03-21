// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.PuppeteerSharp;

/// <summary>
/// PuppeteerOptions
/// </summary>
public class PuppeteerOptions : LoaderOptions
{
    public PuppeteerOptions()
    {
        RefreshMode = LoaderRefreshMode.BasedOnCacheControl;

        ScreenshotWidth = 1400;
        ScreenshotHeight = 900;
        CacheControlMaxAge = TimeSpan.FromDays(7);
    }

    /// <summary>
    /// ScreenshotWidth
    /// </summary>
    public int ScreenshotWidth { get; set; }

    /// <summary>
    /// ScreenshotHeight
    /// </summary>
    public int ScreenshotHeight { get; set; }

   
}
