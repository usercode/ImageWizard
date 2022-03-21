// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SkiaSharp;

namespace ImageWizard.SkiaSharp;

public class PngFormat : IImageFormat
{
    public string MimeType => MimeTypes.Png;

    public void SaveImage(SKBitmap image, Stream stream)
    {
        image.Encode(stream, SKEncodedImageFormat.Png, 85);
    }
}
