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

namespace ImageWizard.ImageSharp.Filters
{
    public class CropFilter : ImageFilter
    {
        [Filter]
        public void Crop(double width, double height)
        {
            Crop(0, 0, width, height);
        }

        [Filter]
        public void Crop(double x, double y, double width, double height)
        {
            Crop(
                (int)(x * Context.Image.Width),
                (int)(y * Context.Image.Height),
                (int)(width * Context.Image.Width),
                (int)(height * Context.Image.Height));
        }

        [Filter]
        public void Crop(int width, int height)
        {
            Crop(0, 0, width, height);
        }

        [Filter]
        public void Crop(int x, int y, int width, int height)
        {
           Context.Image.Mutate(m => m.Crop(new Rectangle(x, y, width, height)));
        }
    }
}
