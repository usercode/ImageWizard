using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.ImageSharp.Filters
{
    /// <summary>
    /// DPRFilter
    /// </summary>
    public class DPRFilter : ImageSharpFilter
    {
        [Filter]
        public void DPR(double dpr)
        {
            Context.ClientHints.DPR = dpr;
        }
    }
}
