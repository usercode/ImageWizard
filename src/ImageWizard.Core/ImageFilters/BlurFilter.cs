using ImageWizard.Core.ImageFilters.Base.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public class BlurFilter : FilterBase
    {
        [Filter]
        public void Blur(FilterContext context)
        {
            context.Image.Mutate(m => m.BoxBlur());
        }
    }
}
