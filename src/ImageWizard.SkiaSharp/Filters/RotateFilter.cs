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
    public class RotateFilter : SkiaSharpFilter
    {
        [Filter]
        public void Rotate(float angle)
        {
            int w;
            int h;

            if (angle < 90)
            {
                w = (int)(Math.Abs(Context.Image.Width * Math.Cos(angle)) + Math.Abs(Context.Image.Height * Math.Sin(angle)));
                h = (int)(Math.Abs(Context.Image.Width * Math.Sin(angle)) + Math.Abs(Context.Image.Height * Math.Cos(angle)));
            }
            else if (angle == 90)
            {
                w = Context.Image.Height;
                h = Context.Image.Width;
            }
            else if(angle == 180)
            {
                w = Context.Image.Width;
                h = Context.Image.Height;
            }
            else if(angle == 270)
            {
                w = Context.Image.Height;
                h = Context.Image.Width;
            }
            else
            {
                throw new Exception();
            }


            using (var surface = SKSurface.Create(new SKImageInfo(w, h)))
            using (var canvas = surface.Canvas)
            {
                canvas.Translate(Math.Abs(w - Context.Image.Width) / 2, Math.Abs(h - Context.Image.Height) / 2);
                canvas.RotateDegrees(angle, Context.Image.Width / 2, Context.Image.Height / 2);

                canvas.DrawBitmap(Context.Image, 0, 0);
                canvas.Flush();

                // save
                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
    }
}
