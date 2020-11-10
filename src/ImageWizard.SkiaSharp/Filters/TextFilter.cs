using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.SkiaSharp.Filters.Base;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.SkiaSharp.Filters
{
    public class TextFilter : SkiaSharpFilter
    {
        [Filter]
        public void DrawText(int x = 0, int y = 0, string text = "", int size = 24, string font = "Arial", string color = "#ff000000")
        {
            using (var surface = SKSurface.Create(new SKImageInfo(Context.Image.Width, Context.Image.Height)))
            using (var canvas = surface.Canvas)
            using (var paint = new SKPaint() { TextSize = size, IsAntialias = true, Color = SKColor.Parse(color)})
            {
                canvas.DrawBitmap(Context.Image, 0, 0);
                canvas.DrawText(text, x, y, new SKFont(SKTypeface.FromFamilyName(font)), paint);
                canvas.Flush();

                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
    }
}
