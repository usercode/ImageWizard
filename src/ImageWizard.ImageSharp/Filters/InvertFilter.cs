﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

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
