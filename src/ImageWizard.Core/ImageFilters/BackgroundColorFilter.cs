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
    public class BackgroundColorFilter : FilterBase
    {
        [Filter]
        public void BackgroundColor(byte r, byte g, byte b, FilterContext context)
        {
            context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b)));
        }

        [Filter]
        public void BackgroundColor(float r, float g, float b, FilterContext context)
        {
            context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b)));
        }
    }
}
