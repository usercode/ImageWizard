using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class JpgFilter : FilterBase
    {
        [Filter]
        public void Jpg(FilterContext context)
        {
            context.ImageFormat = new JpegFormat();
        }

        [Filter]
        public void Jpg(int quality, FilterContext context)
        {
            context.ImageFormat = new JpegFormat() { Quality = quality };
        }
    }
}
