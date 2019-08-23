using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageFilters.Options
{
    /// <summary>
    /// NoCacheFilter
    /// </summary>
    public class NoImageCacheFilter : FilterBase
    {
        [Filter]
        public void NoCache(FilterContext context)
        {
            context.NoImageCache = true;
        }
    }
}
