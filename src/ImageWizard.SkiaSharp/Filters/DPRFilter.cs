using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Filters;
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
            Context.ClientHints.DPR = dpr;
        }
    }
}
