// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public partial class ResizeFilter : ImageSharpFilter
{
    [Filter]
    public void Resize([DPR]int size)
    {
        Resize(size, size);
    }

    [Filter]
    public void Resize([DPR]int width, [DPR]int height)
    {
        Resize(width, height, Utils.ResizeMode.Max);
    }

    [Filter]
    public void Resize([DPR]int width, [DPR]int height, Utils.ResizeMode mode)
    {
        Resize(width, height, mode, Utils.AnchorPositionMode.Center);
    }

    [Filter]
    public void Resize([DPR]int width, [DPR]int height, Utils.ResizeMode mode, Utils.AnchorPositionMode position)
    {
        ResizeMode mode2 = mode switch
        {
            Utils.ResizeMode.Max => ResizeMode.Max,
            Utils.ResizeMode.Min => ResizeMode.Min,
            Utils.ResizeMode.Stretch => ResizeMode.Stretch,
            Utils.ResizeMode.Pad => ResizeMode.Pad,
            Utils.ResizeMode.Crop => ResizeMode.Crop,
            _ => throw new Exception(),
        };

        AnchorPositionMode anchorPositionMode = position switch
        {
            Utils.AnchorPositionMode.Bottom => AnchorPositionMode.Bottom,
            Utils.AnchorPositionMode.BottomLeft => AnchorPositionMode.BottomLeft,
            Utils.AnchorPositionMode.BottomRight => AnchorPositionMode.BottomRight,
            Utils.AnchorPositionMode.Center => AnchorPositionMode.Center,
            Utils.AnchorPositionMode.Left => AnchorPositionMode.Left,
            Utils.AnchorPositionMode.Right => AnchorPositionMode.Right,
            Utils.AnchorPositionMode.Top => AnchorPositionMode.Top,
            Utils.AnchorPositionMode.TopLeft => AnchorPositionMode.TopLeft,
            Utils.AnchorPositionMode.TopRight => AnchorPositionMode.TopRight,
            _ => throw new Exception(),
        };

        //prevent upscaling
        if (width > Context.Image.Width)
        {
            width = Context.Image.Width;
        }

        if (height > Context.Image.Height)
        {
            height = Context.Image.Height;
        }

        Context.Image.Mutate(m => m.Resize(new ResizeOptions()
        {
            Position = anchorPositionMode,
            Mode = mode2,
            Size = new Size(width, height),
            PadColor = Color.White
        }));
    }
}
