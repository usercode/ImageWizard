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
    }
}
