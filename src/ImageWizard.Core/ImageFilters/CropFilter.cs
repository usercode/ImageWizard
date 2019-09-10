using ImageWizard.Core.ImageFilters.Base;
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
    public class CropFilter : FilterBase
    {
        [Filter]
        public void Crop(double width, double height, FilterContext context)
        {
            Crop(0, 0, width, height, context);
        }

        [Filter]
        public void Crop(double x, double y, double width, double height, FilterContext context)
        {
            Crop(
                (int)(x * context.Image.Width),
                (int)(y * context.Image.Height),
                (int)(width * context.Image.Width),
                (int)(height * context.Image.Height),
                context);
        }

        [Filter]
        public void Crop(int width, int height, FilterContext context)
        {
            Crop(0, 0, width, height, context);
        }

        [Filter]
        public void Crop(int x, int y, int width, int height, FilterContext context)
        {
            context.Image.Mutate(m => m.Crop(new Rectangle(x, y, width, height)));
        }
    }
}
