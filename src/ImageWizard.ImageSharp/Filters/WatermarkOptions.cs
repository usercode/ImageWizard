// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;

namespace ImageWizard.ImageSharp.Filters;

public class WatermarkOptions
{
    public WatermarkOptions()
    {
        Margin = 10;
        Opacity = 0.5f;
        Location = WatermarkLocation.BottomRight;
    }

    /// <summary>
    /// Margin
    /// </summary>
    public int Margin { get; set; }

    /// <summary>
    /// Path to the watermark image
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Opacity
    /// </summary>
    public float Opacity { get; set; }

    /// <summary>
    /// Location
    /// </summary>
    public WatermarkLocation Location { get; set; }
}
