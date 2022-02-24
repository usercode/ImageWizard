// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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
    public class FlipFilter : ImageSharpFilter
    {
        [Filter]
        public void Flip(Utils.FlipMode flippingMode)
        {
            var flipMode = flippingMode switch
            {
                Utils.FlipMode.Horizontal => FlipMode.Horizontal,
                Utils.FlipMode.Vertical => FlipMode.Vertical,
                _ => throw new Exception(),
            };

            Context.Image.Mutate(m => m.Flip(flipMode));
        }
    }
}
