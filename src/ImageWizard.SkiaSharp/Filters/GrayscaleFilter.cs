// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.SkiaSharp.Filters.Base;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.SkiaSharp.Filters;

public class GrayscaleFilter : SkiaSharpFilter
{
    [Filter]
    public void Grayscale()
    {
        using (var surface = SKSurface.Create(new SKImageInfo(Context.Image.Width, Context.Image.Height)))
        using (var canvas = surface.Canvas)
        using (var paint = new SKPaint())
        {
            paint.ColorFilter =
                                SKColorFilter.CreateColorMatrix(new float[]
                                {
                                    0.21f, 0.72f, 0.07f, 0, 0,
                                    0.21f, 0.72f, 0.07f, 0, 0,
                                    0.21f, 0.72f, 0.07f, 0, 0,
                                    0,     0,     0,     1, 0
                                });

            canvas.DrawBitmap(Context.Image, 0, 0, paint);
            canvas.Flush();

            // save
            Context.Image = SKBitmap.FromImage(surface.Snapshot());
        }
    }
}
