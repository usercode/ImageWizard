using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.ImageSharp.Filters
{
    /// <summary>
    /// NoCacheFilter
    /// </summary>
    public class NoImageCacheFilter : ImageFilter
    {
        [Filter]
        public void NoCache()
        {
            Context.NoImageCache = true;
        }
    }
}
