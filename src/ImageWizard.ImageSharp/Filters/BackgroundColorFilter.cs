﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

public class BackgroundColorFilter : ImageSharpFilter
{
    [Filter]
    public void BackgroundColor(byte r, byte g, byte b)
    {
        Context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b)));
    }

    [Filter]
    public void BackgroundColor(byte r, byte g, byte b, byte a)
    {
        Context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b, a)));
    }

    [Filter]
    public void BackgroundColor(float r, float g, float b)
    {
        Context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b)));
    }

    [Filter]
    public void BackgroundColor(float r, float g, float b, float a)
    {
        Context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b, a)));
    }
}
