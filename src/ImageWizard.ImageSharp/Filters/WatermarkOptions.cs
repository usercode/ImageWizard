// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;

namespace ImageWizard.ImageSharp.Filters;

public class WatermarkOptions
{
    /// <summary>
    /// Margin
    /// </summary>
    public int Margin { get; set; } = 10;

    /// <summary>
    /// Path to the watermark image
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Opacity
    /// </summary>
    public float Opacity { get; set; } = 0.5f;

    /// <summary>
    /// Location
    /// </summary>
    public WatermarkLocation Location { get; set; } = WatermarkLocation.BottomRight;
}
