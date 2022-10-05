// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SixLabors.ImageSharp;

namespace ImageWizard.ImageSharp;

public class PngFormat : IImageFormat
{
    public string MimeType => MimeTypes.Png;

    public async Task SaveImageAsync(Image image, Stream stream)
    {
        await image.SaveAsPngAsync(stream);
    }
}
