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
    public class AutoOrientFilter : FilterBase
    {
        public override string Name => "autoorient";

        public void Execute(FilterContext context)
        {
            context.Image.Mutate(m => m.AutoOrient());
        }
    }
}
