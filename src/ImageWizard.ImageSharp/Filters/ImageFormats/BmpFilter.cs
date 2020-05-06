using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters.ImageFormats
{
    public class BmpFilter : ImageFilter
    {
        [Filter]
        public void Bmp()
        {
            Context.ImageFormat = new BmpFormat();
        }
    }
}
