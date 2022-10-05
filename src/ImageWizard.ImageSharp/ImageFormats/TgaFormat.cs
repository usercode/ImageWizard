// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SixLabors.ImageSharp;

namespace ImageWizard.ImageSharp;

public class TgaFormat : IImageFormat
{
    public string MimeType => MimeTypes.Tga;

    public async Task SaveImageAsync(Image image, Stream stream)
    {
        await image.SaveAsTgaAsync(stream);
    }
}
