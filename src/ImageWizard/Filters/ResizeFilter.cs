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
            context.Image.Mutate(m =>
            {                
                m.Resize(new ResizeOptions()
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, height)
                });
                m.BackgroundColor(Rgba32.White);
            });
        }
    }
}
