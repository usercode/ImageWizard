using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageFormats.Base
{
    public interface IImageFormat
    {
        string MimeType { get; }

        Task SaveImageAsync(Image image, Stream stream);
    }
}
