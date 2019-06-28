using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using SixLabors.ImageSharp.Primitives;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.Fonts;

namespace ImageWizard.Filters
{
    public class TextFilter : FilterBase
    {
        public override string Name => "text";

        public void Execute(int x, int y, string text, FilterContext context)
        {
            Execute(x, y, text, 12, "Arial", context);
        }

        public void Execute(int x, int y, string text, int size, string font, FilterContext context)
        {
            context.Image.Mutate(m =>
            {
                m.DrawText(
                text,
                new Font(SystemFonts.Find(font), size),
                Rgba32.Black,
                new PointF(x, y));
            });
        }
    }
}
