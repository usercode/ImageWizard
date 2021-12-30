using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.SkiaSharp.Filters.Base;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp.Filters
{
    public class CropFilter : SkiaSharpFilter
    {
        [Filter]
        public void Crop(float width, float height)
        {
            Crop(0, 0, width, height);
        }

        [Filter]
        public void Crop(float x, float y, float width, float height)
        {
            float newWidth = width * Context.Image.Width;
            float newHeight = height * Context.Image.Height;

            float newX = x * Context.Image.Width;
            float newY = y * Context.Image.Height;

            using (var surface = SKSurface.Create(new SKImageInfo((int)newWidth, (int) newHeight)))
            using (var canvas = surface.Canvas)
            {
                SKRect sourceRect = new SKRect(newX, newY, newX + newWidth, newY + newHeight);
                SKRect destRect = new SKRect(0, 0, newWidth, newHeight);

                canvas.DrawBitmap(Context.Image, sourceRect, destRect);

                // save
                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
    }
}
