// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SkiaSharp;

namespace ImageWizard.SkiaSharp;

public interface IImageFormat
{
    string MimeType { get; }

    void SaveImage(SKBitmap image, Stream stream);
}
