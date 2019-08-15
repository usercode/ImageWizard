using ImageWizard.Core.ImageFilters.Base;
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
    public class CropFilter : FilterBase
    {
        public override string Name => "crop";

        public void Execute([DPR]double width, [DPR]double height, FilterContext context)
        {
            Execute(0, 0, width, height, context);
        }

        public void Execute([DPR]double x, [DPR]double y, [DPR]double width, [DPR]double height, FilterContext context)
        {
            Execute(
                (int)(x * context.Image.Width),
                (int)(y * context.Image.Height),
                (int)(width * context.Image.Width),
                (int)(height * context.Image.Height),
                context);
        }

        public void Execute([DPR]int width, [DPR]int height, FilterContext context)
        {
            Execute(0, 0, width, height, context);
        }

        public void Execute([DPR]int x, [DPR]int y, [DPR]int width, [DPR]int height, FilterContext context)
        {
            context.Image.Mutate(m => m.Crop(new Rectangle(x, y, width, height)));
        }
    }
}
