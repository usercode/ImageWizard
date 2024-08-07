﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

public partial class ImageFormatFilter : ImageSharpFilter
{
    [Filter]
    public void Bmp()
    {
        Context.ImageFormat = new BmpFormat();
    }

    [Filter]
    public void Gif()
    {
        Context.ImageFormat = new GifFormat();
    }

    [Filter]
    public void Jpg()
    {
        Context.ImageFormat = new JpegFormat();
    }

    [Filter]
    public void Jpg(int quality)
    {
        Context.ImageFormat = new JpegFormat() { Quality = quality };
    }

    [Filter]
    public void WebP()
    {
        Context.ImageFormat = new WebPFormat();
    }

    [Filter]
    public void WebP(int quality)
    {
        Context.ImageFormat = new WebPFormat() { Quality = quality };
    }

    [Filter]
    public void WebP(int quality, bool lossless)
    {
        Context.ImageFormat = new WebPFormat() { Quality = quality, Lossless = lossless };
    }

    [Filter]
    public void Png()
    {
        Context.ImageFormat = new PngFormat();
    }

    [Filter]
    public void Tga()
    {
        Context.ImageFormat = new TgaFormat();
    }
}
