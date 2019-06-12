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
    public class TrimFilter : FilterBase
    {
        public override string Name => "trim";

        public void Execute(FilterContext context)
        {
            //find whitespace

            int top = 0;
            int left = 0;
            int bottom = 0;
            int right = 0;

            
            //context.Image.Mutate(m => m.Crop(new Rectangle(x, y, width, height)));
        }
    }
}
