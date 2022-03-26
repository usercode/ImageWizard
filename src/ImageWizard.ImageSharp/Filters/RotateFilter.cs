// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
