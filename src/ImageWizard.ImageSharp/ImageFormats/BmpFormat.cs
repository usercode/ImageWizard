// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.ImageSharp;

public class BmpFormat : IImageFormat
{
    public string MimeType => MimeTypes.Bmp;

    public async Task SaveImageAsync(Image image, Stream stream)
    {
        await image.SaveAsBmpAsync(stream);
    }
}
