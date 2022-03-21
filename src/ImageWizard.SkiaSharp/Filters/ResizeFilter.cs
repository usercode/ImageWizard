// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.SkiaSharp.Filters.Base;
using ImageWizard.Utils;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp.Filters;

public class ResizeFilter : SkiaSharpFilter
{
    [Filter]
    public void Resize([DPR]int width, [DPR]int height)
    {
        Resize(width, height, ResizeMode.Max);
    }

    [Filter]
    public void Resize([DPR]int width, [DPR]int height, ResizeMode resizeMode)
    {
        if (resizeMode == ResizeMode.Stretch)
        {
            using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
            using (var canvas = surface.Canvas)
            {
                SKRect sourceRect = new SKRect(0, 0, Context.Image.Width, Context.Image.Height);
                SKRect destRect = new SKRect(0, 0, width, height);

                canvas.DrawBitmap(Context.Image, sourceRect, destRect);

                // save
                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
        else if (resizeMode == ResizeMode.Crop)
        {
            float ratioWidth = (float)width / Context.Image.Width;
            float ratioHeight = (float)height / Context.Image.Height;

            float scale = Math.Max(ratioWidth, ratioHeight);

            int newWidth = (int)(Context.Image.Width * scale);
            int newHeight = (int)(Context.Image.Height * scale);

            using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
            using (var canvas = surface.Canvas)
            {
                float x = (width - scale * Context.Image.Width) / 2;
                float y = (height - scale * Context.Image.Height) / 2;

                SKRect sourceRect = new SKRect(0, 0, Context.Image.Width, Context.Image.Height);
                SKRect destRect = new SKRect(x, y, x + newWidth, y + newHeight);

                canvas.DrawBitmap(Context.Image, sourceRect, destRect);

                // save
                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
        else if (resizeMode == ResizeMode.Pad)
        {
            float ratioWidth = (float)width / Context.Image.Width;
            float ratioHeight = (float)height / Context.Image.Height;

            float scale = Math.Min(ratioWidth, ratioHeight);

            int newWidth = (int)(Context.Image.Width * scale);
            int newHeight = (int)(Context.Image.Height * scale);

            using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
            using (var canvas = surface.Canvas)
            {
                float x = (width - scale * Context.Image.Width) / 2;
                float y = (height - scale * Context.Image.Height) / 2;

                SKRect sourceRect = new SKRect(0, 0, Context.Image.Width, Context.Image.Height);
                SKRect destRect = new SKRect(x, y, x + newWidth, y + newHeight);

                canvas.DrawBitmap(Context.Image, sourceRect, destRect);

                // save
                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
        else if (resizeMode == ResizeMode.Min)
        {
            float ratioWidth = (float)width / Context.Image.Width;
            float ratioHeight = (float)height / Context.Image.Height;

            float scale = Math.Max(ratioWidth, ratioHeight);

            int newWidth = (int)(Context.Image.Width * scale);
            int newHeight = (int)(Context.Image.Height * scale);

            using (var surface = SKSurface.Create(new SKImageInfo(newWidth, newHeight)))
            using (var canvas = surface.Canvas)
            {
                SKRect sourceRect = new SKRect(0, 0, Context.Image.Width, Context.Image.Height);
                SKRect destRect = new SKRect(0, 0, newWidth, newHeight);

                canvas.DrawBitmap(Context.Image, sourceRect, destRect);

                // save
                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
        else if(resizeMode == ResizeMode.Max)
        {
            float ratioWidth = (float)width / Context.Image.Width;
            float ratioHeight = (float)height / Context.Image.Height;

            float scale = Math.Min(ratioWidth, ratioHeight);

            int newWidth = (int)(Context.Image.Width * scale);
            int newHeight = (int)(Context.Image.Height * scale);

            using (var surface = SKSurface.Create(new SKImageInfo(newWidth, newHeight)))
            using (var canvas = surface.Canvas)
            {
                SKRect sourceRect = new SKRect(0, 0, Context.Image.Width, Context.Image.Height);
                SKRect destRect = new SKRect(0, 0, newWidth, newHeight);

                canvas.DrawBitmap(Context.Image, sourceRect, destRect);

                // save
                Context.Image = SKBitmap.FromImage(surface.Snapshot());
            }
        }
    }
}
