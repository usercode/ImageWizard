// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

public class RotateFilter : ImageSharpFilter
{
    [Filter]
    public void Rotate(float angle)
    {
        var rotateMode = angle switch
        {
            90 => RotateMode.Rotate90,
            180 => RotateMode.Rotate180,
            270 => RotateMode.Rotate270,
            _ => throw new Exception("angle is not supported: " + angle),
        };
        Context.Image.Mutate(m => m.Rotate(rotateMode));
    }
}
