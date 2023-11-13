﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

public class AutoOrientFilter : ImageSharpFilter
{
    [Filter]
    public void AutoOrient()
    {
        Context.Image.Mutate(m => m.AutoOrient());
    }
}
