﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;

namespace ImageWizard.Playwright;

/// <summary>
/// PlaywrightOptions
/// </summary>
public class PlaywrightOptions : LoaderOptions
{
    public PlaywrightOptions()
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
