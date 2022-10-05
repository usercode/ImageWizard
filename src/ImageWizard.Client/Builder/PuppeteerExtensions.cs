// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

/// <summary>
/// PuppeteerExtensions
/// </summary>
public static class PuppeteerExtensions
{
    public static Image Screenshot(this ILoader imageUrlBuilder, string source)
    {
        return new Image(imageUrlBuilder.LoadData("screenshot", source));
    }
}
