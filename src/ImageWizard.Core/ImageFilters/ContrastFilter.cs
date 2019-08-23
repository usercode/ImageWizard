using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public class ContrastFilter : FilterBase
    {
        public override string Name => "contrast";

        public void Execute(float value, FilterContext context)
        {
            context.Image.Mutate(m => m.Contrast(value));
        }
    }
}
