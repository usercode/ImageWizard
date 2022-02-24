// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.SkiaSharp.Filters.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp.Filters
{
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
}
