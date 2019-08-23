using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class BmpFilter : FilterBase
    {
        [Filter]
        public void Bmp(FilterContext context)
        {
            context.ImageFormat = new BmpFormat();
        }
    }
}
