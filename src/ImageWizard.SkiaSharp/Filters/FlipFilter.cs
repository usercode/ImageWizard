using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.SkiaSharp.Filters.Base;
using ImageWizard.Utils.FilterTypes;
using Microsoft.AspNetCore.Routing.Constraints;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp.Filters
{
    public class FlipFilter : SkiaSharpFilter
    {
        [Filter]
        public void Flip(FlipMode flipMode)
        {
            using (var surface = SKSurface.Create(new SKImageInfo(Context.Image.Width, Context.Image.Height)))
            using (var canvas = surface.Canvas)
            {
                SKRect sourceRect = sourceRect = new SKRect(0, 0, Context.Image.Width, Context.Image.Height);
                SKRect destRect = new SKRect(0, 0, Context.Image.Width, Context.Image.Height);

                if (flipMode == FlipMode.Horizontal)
                {
                    canvas.Scale(-1, 1, Context.Image.Width / 2.0f, 0);
                }
                else if(flipMode == FlipMode.Vertical)
                {
                    canvas.Scale(1, -1, 0, Context.Image.Height / 2.0f);
                }
                else
                {
                    throw new Exception("unknown flip mode");
                }               

                canvas.DrawBitmap(Context.Image, sourceRect, destRect);

                // save
                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
    }
}
