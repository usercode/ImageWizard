// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace ImageWizard.ImageSharp;

public class WebPFormat : IImageFormat
{
    public WebPFormat()
    {
        Quality = 80;
    }

    /// <summary>
    /// Quality
    /// </summary>
    public int Quality { get; set; }

    /// <summary>
    /// MimeType
    /// </summary>
    public string MimeType => MimeTypes.WebP;

    public async Task SaveImageAsync(Image image, Stream stream)
    {
        await image.SaveAsWebpAsync(stream, new WebpEncoder { Quality = Quality });
    }
}
