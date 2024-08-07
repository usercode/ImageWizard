﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public partial class BlurFilter : ImageSharpFilter
{
    [Filter]
    public void Blur()
    {
        Blur(10);
    }

    [Filter]
    public void Blur(int radius)
    {
        Context.Image.Mutate(m => m.BoxBlur(radius));
    }
}
