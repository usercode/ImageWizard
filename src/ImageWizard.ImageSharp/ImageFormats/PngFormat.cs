// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.ImageSharp;

public class PngFormat : IImageFormat
{
    public string MimeType => MimeTypes.Png;

    public async Task SaveImageAsync(Image image, Stream stream)
    {
        await image.SaveAsPngAsync(stream);
    }
}
