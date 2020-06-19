using ImageWizard.Core.ImageFilters.Base.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    public class BlurFilter : ImageSharpFilter
    {
        [Filter]
        public void Blur()
        {
            Blur(10);
        }

        [Filter]
        public void Blur(int radius)
        {
            Context.Image.Mutate(m => m.BoxBlur(radius));
        }
    }
}
