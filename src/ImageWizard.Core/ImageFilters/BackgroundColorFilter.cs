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
        public override string Name => "backgroundcolor";

        public void Execute(int r, int g, int b, FilterContext context)
        {
            context.Image.Mutate(m => m.BackgroundColor(new Rgba32((byte)r, (byte)g, (byte)b)));
        }

        public void Execute(double r, double g, double b, FilterContext context)
        {
            context.Image.Mutate(m => m.BackgroundColor(new Rgba32((float)r, (float)g, (float)b)));
        }
    }
}
