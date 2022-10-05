// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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
