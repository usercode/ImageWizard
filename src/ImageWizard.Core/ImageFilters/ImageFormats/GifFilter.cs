using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class GifFilter : FilterBase
    {
        [Filter]
        public void Gif(FilterContext context)
        {
            context.ImageFormat = new GifFormat();
        }
    }
}
