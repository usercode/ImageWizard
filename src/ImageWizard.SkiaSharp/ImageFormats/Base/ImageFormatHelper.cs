// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.SkiaSharp;

class ImageFormatHelper
{
    public static IImageFormat FirstOrDefault(string mimeType)
    {
        return FirstOrDefault(new[] { mimeType });
    }

    public static IImageFormat FirstOrDefault(IEnumerable<string> mimeTypes)
    {
        foreach (string mimeType in mimeTypes)
        {
            IImageFormat imageFormat = mimeType switch
            {
                MimeTypes.Jpeg => new JpegFormat(),
                MimeTypes.Png => new PngFormat(),
                MimeTypes.Gif => new GifFormat(),
                MimeTypes.Bmp => new BmpFormat(),
                MimeTypes.WebP => new WebPFormat(),
                _ => null,
            };

            if (imageFormat != null)
            {
                return imageFormat;
            }
        }

        return null;
    }
}
