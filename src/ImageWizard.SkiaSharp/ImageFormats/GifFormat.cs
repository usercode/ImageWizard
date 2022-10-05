// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SkiaSharp;

namespace ImageWizard.SkiaSharp;

public class GifFormat : IImageFormat
{
    public string MimeType => MimeTypes.Gif;

    public void SaveImage(SKBitmap image, Stream stream)
    {
        image.Encode(stream, SKEncodedImageFormat.Gif, 85);
    }
}
