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
        public override string Name => "resize";

        public void Execute(int size, FilterContext context)
        {
            Execute(size, size, context);
        }

        public void Execute(int width, int height, FilterContext context)
        {
            Execute(width, height, SharedContract.FilterTypes.ResizeMode.Max, context);
        }

        public void Execute(int width, int height, SharedContract.FilterTypes.ResizeMode mode, FilterContext context)
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

            context.Image.Mutate(m =>
            {                
                m.Resize(new ResizeOptions()
                {
                    Mode = mode2,
                    Size = new Size(width, height)
                });
                m.BackgroundColor(Rgba32.White);
            });
        }
    }
}
