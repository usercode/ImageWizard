// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.SkiaSharp.Filters.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.SkiaSharp.Filters
{
    public class ImageFormatFilter : SkiaSharpFilter
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
        public void Png()
        {
            Context.ImageFormat = new PngFormat();
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
    }
}
