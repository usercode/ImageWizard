// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageWizard.ImageSharp;

public class GifFormat : IImageFormat
{
    public string MimeType => MimeTypes.Gif;

    public async Task SaveImageAsync(Image image, Stream stream)
    {
        await image.SaveAsGifAsync(stream);
    }
}
