// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageWizard.Playwright;

/// <summary>
/// ScreenshotLoader
/// </summary>
public class ScreenshotLoader : Loader<PlaywrightOptions>
{
    public ScreenshotLoader(ILogger<ScreenshotLoader> logger, IOptions<PlaywrightOptions> options)
        : base(options)
    {
        Logger = logger;
    }

    /// <summary>
    /// Logger
    /// </summary>
    public ILogger<ScreenshotLoader> Logger { get; }

    public override async Task<LoaderResult> GetAsync(string source, CachedData? existingCachedData)
    {
        using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
        var page = await browser.NewPageAsync();
        await page.SetViewportSizeAsync(Options.Value.ScreenshotWidth, Options.Value.ScreenshotHeight);
        await page.GotoAsync(source);
        byte[] buffer = await page.ScreenshotAsync();

        return LoaderResult.Success(new OriginalData(
                                                MimeTypes.Png, 
                                                new MemoryStream(buffer), 
                                                new CacheSettings().ApplyLoaderOptions(Options.Value)));
    }
}
