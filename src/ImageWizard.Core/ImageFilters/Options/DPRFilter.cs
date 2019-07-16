using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageFilters.Options
{
    public class DPRFilter : FilterBase
    {
        public override string Name => "dpr";

        public void Execute(double dpr, FilterContext context)
        {
            context.DPR = dpr;
        }
    }
}
