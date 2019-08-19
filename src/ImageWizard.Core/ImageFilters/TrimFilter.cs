using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public class TrimFilter : FilterBase
    {
        public override string Name => "trim";

        public void Execute(FilterContext context)
        {
            //find whitespace

            int top = 0;
            int left = 0;
            int bottom = 0;
            int right = 0;

            Task topTask = Task.Run(() =>
            {
                for(int y = 0; y < context.Image.Height; y++)
                {
                    for(int x = 0; x < context.Image.Width; x++)
                    {
                        if(context.Image[x,y] != Rgba32.White)
                        {
                            top = y;
                            break;
                        }
                    }
                }
            });

            Task bottomTask = Task.Run(() =>
            {
                for (int y = context.Image.Height; y >= 0; y--)
                {
                    for (int x = 0; x < context.Image.Width; x++)
                    {
                        if (context.Image[x, y] != Rgba32.White)
                        {
                            bottom = y;
                            break;
                        }
                    }
                }
            });
            
            Task.WhenAll(topTask).Wait();
            
            //context.Image.Mutate(m => m.Crop(new Rectangle(x, y, width, height)));
        }
    }
}
