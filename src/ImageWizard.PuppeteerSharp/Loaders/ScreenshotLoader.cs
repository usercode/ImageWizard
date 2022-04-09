﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageWizard.PuppeteerSharp;

/// <summary>
/// ScreenshotLoader
/// </summary>
public class ScreenshotLoader : Loader<PuppeteerOptions>
{
    public ScreenshotLoader(ILogger<ScreenshotLoader> logger, IOptions<PuppeteerOptions> options)
        : base(options)
    {
        Logger = logger;
    }

    /// <summary>
    /// Logger
    /// </summary>
    public ILogger<ScreenshotLoader> Logger { get; }

    public override async Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedData)
    {
        using BrowserFetcher browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();

        await using Browser browser = await Puppeteer.LaunchAsync(
                                                                    new LaunchOptions()
                                                                    { 
                                                                        Headless = true,
                                                                        Args = new[] { "--no-sandbox" }
                                                                    });

        Logger.LogInformation("Chrome version: {version}", await browser.GetVersionAsync());

        await using Page page = await browser.NewPageAsync();

        await page.SetViewportAsync(new ViewPortOptions() 
                                    { 
                                        Width = Options.Value.ScreenshotWidth, 
                                        Height = Options.Value.ScreenshotHeight 
                                    });

        await page.GoToAsync(source);

        byte[] buffer = await page.ScreenshotDataAsync(new ScreenshotOptions() { Type = ScreenshotType.Png });

        return new OriginalData(MimeTypes.Png, new MemoryStream(buffer), new CacheSettings().ApplyLoaderOptions(Options.Value));
    }
}