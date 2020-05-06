using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters.ImageFormats
{
    public class PngFilter : ImageFilter
    {
        [Filter]
        public void Png()
        {
            Context.ImageFormat = new PngFormat();
        }
    }
}
