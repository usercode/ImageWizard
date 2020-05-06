using ImageWizard.Core.ImageFilters.Base.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    public class FlipFilter : ImageFilter
    {
        [Filter]
        public void Flip(SharedContract.FilterTypes.FlipMode flippingMode)
        {
            FlipMode flipMode;

            switch(flippingMode)
            {
                case SharedContract.FilterTypes.FlipMode.Horizontal:
                    flipMode = FlipMode.Horizontal;
                    break;

                case SharedContract.FilterTypes.FlipMode.Vertical:
                    flipMode = FlipMode.Vertical;
                    break;

                default:
                    throw new Exception();
            }

            Context.Image.Mutate(m => m.Flip(flipMode));
        }
    }
}
