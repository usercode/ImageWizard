// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;

namespace ImageWizard.ImageSharp;

public class PlaceholderOptions : LoaderOptions
{
    public string BackgroundColor { get; set; } = "#4682b4";

    public string FontColor { get; set; } = "#FFFFFF";

    public string FontName { get; set; } = "Arial";

    public float FontSize { get; set; } = 32;

}
