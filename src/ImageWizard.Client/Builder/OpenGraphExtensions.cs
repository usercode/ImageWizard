// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

/// <summary>
/// OpenGraphExtensions
/// </summary>
public static class OpenGraphExtensions
{
    public static Image OpenGraph(this ILoader imageUrlBuilder, string source)
    {
        return new Image(imageUrlBuilder.LoadData("opengraph", source));
    }
}
