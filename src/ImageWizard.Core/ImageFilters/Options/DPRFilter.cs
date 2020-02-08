using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageFilters.Options
{
    /// <summary>
    /// DPRFilter
    /// </summary>
    public class DPRFilter : FilterBase
    {
        [Filter]
        public void DPR(double dpr, FilterContext context)
        {
            context.ClientHints.DPR = dpr;
        }
    }
}
