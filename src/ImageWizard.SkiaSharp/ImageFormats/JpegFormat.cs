// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SkiaSharp;

namespace ImageWizard.SkiaSharp;

public class JpegFormat : IImageFormat
{
    public JpegFormat()
    {
        Quality = 85;
    }

    /// <summary>
    /// Quality
    /// </summary>
    public int Quality { get; set; }

    public string MimeType => MimeTypes.Jpeg;

    public void SaveImage(SKBitmap image, Stream stream)
    {
        image.Encode(stream, SKEncodedImageFormat.Jpeg, Quality);
    }
}
