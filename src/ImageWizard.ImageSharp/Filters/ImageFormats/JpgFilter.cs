using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters.ImageFormats
{
    public class JpgFilter : ImageFilter
    {
        [Filter]
        public void Jpg()
        {
            Context.ImageFormat = new JpegFormat();
        }

        [Filter]
        public void Jpg(int quality)
        {
            Context.ImageFormat = new JpegFormat() { Quality = quality };
        }
    }
}
