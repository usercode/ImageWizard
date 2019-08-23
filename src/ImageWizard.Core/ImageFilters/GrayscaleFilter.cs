using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public class GrayscaleFilter : FilterBase
    {
        public override string Name => "grayscale";

        public void Execute(FilterContext context)
        {
            context.Image.Mutate(m => m.Grayscale());
        }

        public void Execute(float amount, FilterContext context)
        {
            context.Image.Mutate(m => m.Grayscale(amount));
        }
    }
}
