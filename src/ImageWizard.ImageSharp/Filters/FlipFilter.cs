using ImageWizard.Core.ImageFilters.Base.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    public class FlipFilter : ImageFilter
    {
        [Filter]
        public void Flip(Utils.FilterTypes.FlipMode flippingMode)
        {
            FlipMode flipMode;

            switch(flippingMode)
            {
                case Utils.FilterTypes.FlipMode.Horizontal:
                    flipMode = FlipMode.Horizontal;
                    break;

                case Utils.FilterTypes.FlipMode.Vertical:
                    flipMode = FlipMode.Vertical;
                    break;

                default:
                    throw new Exception();
            }

            Context.Image.Mutate(m => m.Flip(flipMode));
        }
    }
}
