using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System;
using System.Threading.Tasks;
using SixLabors.Fonts;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageWizard.ImageSharp.Filters
{
    public class TextFilter : ImageSharpFilter
    {
        [Filter]
        public void DrawText(float x = 0, float y = 0, string text = "", int size = 24, string font = "Arial")
        {
            Context.Image.Mutate(m =>
             {
                 m.DrawText(
                     text,
                     new Font(SystemFonts.Find(font), size),
                     Color.Black,
                     new PointF(Context.Image.Width * x, Context.Image.Height * y));
             });
        }
    }
}
