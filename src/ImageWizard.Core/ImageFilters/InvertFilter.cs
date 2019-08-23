using ImageWizard.Core.ImageFilters.Base;
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
    public class InvertFilter : FilterBase
    {
        public override string Name => "invert";

        public void Execute(FilterContext context)
        {
            context.Image.Mutate(m => m.Invert());
        }

        public void Execute([DPR]int x, [DPR]int y, [DPR]int width, [DPR]int height, FilterContext context)
        {
            context.Image.Mutate(m => m.Invert(new SixLabors.Primitives.Rectangle(x, y, width, height)));
        }
    }
}
