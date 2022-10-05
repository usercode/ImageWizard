// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.SkiaSharp.Filters.Base;

namespace ImageWizard.SkiaSharp.Filters;

/// <summary>
/// DPRFilter
/// </summary>
public class DPRFilter : SkiaSharpFilter
{
    [Filter]
    public void DPR(double dpr)
    {
        Context.ProcessingContext.ClientHints.DPR = dpr;
    }
}
