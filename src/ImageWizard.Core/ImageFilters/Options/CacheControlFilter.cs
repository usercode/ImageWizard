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
    public class CacheControlFilter : FilterBase
    {
        [Filter]
        public void CacheControl(int maxAge = 2, int noCache = 0, bool isCached = true, FilterContext context = null)
        {
            context.NoImageCache = true;
        }
    }
}
