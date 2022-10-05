// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

/// <summary>
/// DPRFilter
/// </summary>
public class DPRFilter : ImageSharpFilter
{
    [Filter]
    public void DPR(double dpr)
    {
        Context.ProcessingContext.ClientHints.DPR = dpr;
    }
}
