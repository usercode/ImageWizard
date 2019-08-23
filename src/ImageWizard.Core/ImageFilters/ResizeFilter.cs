using ImageWizard.Core.ImageFilters.Base;
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
    public class ResizeFilter : FilterBase
    {
        [Filter]
        public void Resize([DPR]int size, FilterContext context)
        {
            Resize(size, size, context);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height, FilterContext context)
        {
            Resize(width, height, SharedContract.FilterTypes.ResizeMode.Max, context);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height, SharedContract.FilterTypes.ResizeMode mode, FilterContext context)
        {
            ResizeMode mode2;

            switch (mode)
            {
                case SharedContract.FilterTypes.ResizeMode.Max:
                    mode2 = ResizeMode.Max;
                    break;

                case SharedContract.FilterTypes.ResizeMode.Min:
                    mode2 = ResizeMode.Min;
                    break;

                case SharedContract.FilterTypes.ResizeMode.Stretch:
                    mode2 = ResizeMode.Stretch;
                    break;

                case SharedContract.FilterTypes.ResizeMode.Pad:
                    mode2 = ResizeMode.Pad;
                    break;

                case SharedContract.FilterTypes.ResizeMode.Crop:
                    mode2 = ResizeMode.Crop;
                    break;

                default:
                    throw new Exception();
            }

            //prevent upscaling
            if(width > context.Image.Width)
            {
                width = context.Image.Width;
            }

            if(height > context.Image.Height)
            {
                height = context.Image.Height;
            }

            context.Image.Mutate(m => m.Resize(new ResizeOptions()
                                                {
                                                    Mode = mode2,
                                                    Size = new Size(width, height)
                                                }));
        }
    }
}
