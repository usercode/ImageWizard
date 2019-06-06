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
    public class BlackWhiteFilter : FilterBase
    {
        public override string Name => "blackwhite";

        public void Execute(FilterContext context)
        {
            context.Image.Mutate(m => m.BlackWhite());
        }

       
    }
}
