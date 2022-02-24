// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp
{
    public interface IImageFormat
    {
        string MimeType { get; }

        Task SaveImageAsync(Image image, Stream stream);
    }
}
