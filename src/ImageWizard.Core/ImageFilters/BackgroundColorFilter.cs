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

        public void Execute(byte r, byte g, byte b, FilterContext context)
        {
            context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b)));
        }

        public void Execute(float r, float g, float b, FilterContext context)
        {
            context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b)));
        }
    }
}
