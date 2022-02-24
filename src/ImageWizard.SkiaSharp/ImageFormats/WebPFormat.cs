// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SkiaSharp;

namespace ImageWizard.SkiaSharp
{
    public class WebPFormat : IImageFormat
    {
        public WebPFormat()
        {
            Quality = 85;
        }

        /// <summary>
        /// Quality
        /// </summary>
        public int Quality { get; set; }

        public string MimeType => MimeTypes.WebP;

        public void SaveImage(SKBitmap image, Stream stream)
        {
            image.Encode(stream, SKEncodedImageFormat.Webp, Quality);
        }
    }
}
