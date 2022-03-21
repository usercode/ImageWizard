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

public class InvertFilter : ImageSharpFilter
{
    [Filter]
    public void Invert()
    {
        Context.Image.Mutate(m => m.Invert());
    }

    [Filter]
    public void Invert([DPR]int x, [DPR]int y, [DPR]int width, [DPR]int height)
    {
        Context.Image.Mutate(m => m.Invert(new Rectangle(x, y, width, height)));
    }
}
