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
    public class BlurFilter : ImageFilter
    {
        [Filter]
        public void Blur()
        {
            Context.Image.Mutate(m => m.BoxBlur());
        }
    }
}
