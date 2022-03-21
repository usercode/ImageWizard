﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters;

public abstract class ImageSharpFilter : Filter<ImageSharpFilterContext>
{
    public ImageSharpFilter()
    {

    }

    public override string Namespace => "imagesharp";
}
