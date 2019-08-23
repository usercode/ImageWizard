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
using ImageWizard.Core.ImageFilters.Base.Attributes;

namespace ImageWizard.Filters
{
    public class TextFilter : FilterBase
    {
        [Filter]
        public void DrawText(int x = 0, int y = 0, string text = "", int size = 12, string font = "Arial", FilterContext context = null)
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
