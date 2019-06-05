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

        public void Execute(int x, int y, int width, int height, FilterContext context)
        {
            context.Image.Mutate(m => m.Crop(new Rectangle(x, y, width, height)));
        }
    }
}
