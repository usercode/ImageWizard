using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageFilters.Options
{
    /// <summary>
    /// NoCacheFilter
    /// </summary>
    public class NoCacheFilter : FilterBase
    {
        public override string Name => "nocache";

        public void Execute(FilterContext context)
        {
            context.NoCache = true;
        }
    }
}
