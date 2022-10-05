// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

/// <summary>
/// OpenStreetMapExtensions
/// </summary>
public static class OpenStreetMapExtensions
{
    public static Image OpenStreetMap(this ILoader imageUrlBuilder, int z, int x, int y)
    {
        return new Image(imageUrlBuilder.LoadData("openstreetmap", $"{z}/{x}/{y}.png"));
    }
}
