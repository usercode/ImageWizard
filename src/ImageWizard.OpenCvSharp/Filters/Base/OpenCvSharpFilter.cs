// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.OpenCvSharp.Filters;

/// <summary>
/// OpenCvSharpFilter
/// </summary>
public abstract class OpenCvSharpFilter : Filter<OpenCvSharpFilterContext>
{
    public OpenCvSharpFilter()
    {

    }

    public override string Namespace => "opencvsharp";
}
