// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

/// <summary>
/// OpenGraphExtensions
/// </summary>
public static class PlaceholderExtensions
{
    public static Image Placeholder(this ILoader imageUrlBuilder, int width, int height)
    {
        return new Image(imageUrlBuilder.LoadData("placeholder", $"{width}x{height}"));
    }
}
