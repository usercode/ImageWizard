using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.SkiaSharp
{
    public interface IImageFormat
    {
        string MimeType { get; }

        void SaveImage(SKBitmap image, Stream stream);
    }
}
