using ImageWizard.Core.ImageFilters.Base.Attributes;
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
    public class FlipFilter : FilterBase
    {
        [Filter]
        public void Flip(SharedContract.FilterTypes.FlipMode flippingMode, FilterContext context)
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

            context.Image.Mutate(m => m.Flip(flipMode));
        }
    }
}
