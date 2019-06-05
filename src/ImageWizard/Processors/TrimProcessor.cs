using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Processors
{
    public class TrimProcessor : IImageProcessor<Rgba32>
    {
        public void Apply(Image<Rgba32> source, Rectangle sourceRectangle)
        {
        }
        
    }
}
