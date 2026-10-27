// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace ImageWizard.ImageSharp;

public class WebPFormat : IImageFormat
{
    /// <summary>
    /// Quality
    /// </summary>
    public int Quality { get; set; } = 80;

    /// <summary>
    /// Lossless
    /// </summary>
    public bool Lossless { get; set; } = false;

    /// <summary>
    /// MimeType
    /// </summary>
    public string MimeType => MimeTypes.WebP;

    public async Task SaveImageAsync(Image image, Stream stream)
    {
        await image.SaveAsWebpAsync(stream, new WebpEncoder { Quality = Quality, FileFormat = Lossless ? WebpFileFormatType.Lossless : WebpFileFormatType.Lossy });
    }
}
