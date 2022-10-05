// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SixLabors.ImageSharp;

namespace ImageWizard.ImageSharp;

public class GifFormat : IImageFormat
{
    public string MimeType => MimeTypes.Gif;

    public async Task SaveImageAsync(Image image, Stream stream)
    {
        await image.SaveAsGifAsync(stream);
    }
}
