// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.Utils;
using Microsoft.Extensions.Options;

namespace ImageWizard.ImageSharp.Filters;

public class WatermarkFilter : ImageSharpFilter
{
    public WatermarkFilter(IOptions<WatermarkOptions> options)
    {
        Options = options.Value;
    }

    /// <summary>
    /// Options
    /// </summary>
    private WatermarkOptions Options { get; }

    [Filter]
    public void Watermark(WatermarkLocation location)
    {
        if (string.IsNullOrEmpty(Options.Image))
        {
            throw new Exception("Watermark image is missing.");
        }

        using Image watermark = Image.Load(Options.Image);

        Point point = location switch
        {
            WatermarkLocation.TopLeft => new Point(Options.Margin, Options.Margin),
            WatermarkLocation.TopRight => new Point(Context.Image.Width - watermark.Width - Options.Margin, Options.Margin),
            WatermarkLocation.BottomLeft => new Point(Options.Margin, Context.Image.Height - watermark.Height - Options.Margin),
            WatermarkLocation.BottomRight => new Point(Context.Image.Width - watermark.Width - Options.Margin, Context.Image.Height - watermark.Height - Options.Margin),
            _ => throw new Exception($"Unknown watermark location: {location}")
        };

        Context.Image.Mutate(m => m.DrawImage(watermark, point, Options.Opacity));
    }

    [Filter]
    public void Watermark()
    {
        Watermark(Options.Location);
    }
}
