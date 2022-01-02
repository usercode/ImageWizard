using ImageWizard.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    public class BlackWhiteFilter : ImageSharpFilter
    {
        [Filter]
        public void BlackWhite()
        {
            Context.Image.Mutate(m => m.BlackWhite());
        }
    }
}