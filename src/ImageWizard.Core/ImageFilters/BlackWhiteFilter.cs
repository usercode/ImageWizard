using ImageWizard.Core.ImageFilters.Base.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public class BlackWhiteFilter : FilterBase
    {
        [Filter]
        public void BlackWhite(FilterContext context)
        {
            context.Image.Mutate(m => m.BlackWhite());
        }
    }
}