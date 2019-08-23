using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public class BrightnessFilter : FilterBase
    {
        public override string Name => "brightness";

        public void Execute(float value, FilterContext context)
        {
            context.Image.Mutate(m => m.Brightness(value));
        }
    }
}
