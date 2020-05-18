using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.SkiaSharp.Filters.Base;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.SkiaSharp.Filters
{
    public class BlurFilter : SkiaSharpFilter
    {
        [Filter]
        public void Blur()
        {
            Blur(10);
        }

        [Filter]
        public void Blur(int radius)
        {
            using (var surface = SKSurface.Create(new SKImageInfo(Context.Image.Width, Context.Image.Height)))
            using (var canvas = surface.Canvas)
            using (var paint = new SKPaint())
            {
                paint.ImageFilter = SKImageFilter.CreateBlur(radius, radius);

                SKRect rect = new SKRect(0, 0, Context.Image.Width, Context.Image.Height);
                rect.Inflate(10, 10); //removes black border

                canvas.DrawBitmap(Context.Image, rect, paint);
                canvas.Flush();

                // save
                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
    }
}
