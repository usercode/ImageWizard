using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageWizard.Core.Types;
using ImageWizard.ImageFormats.Base;
using SkiaSharp;

namespace ImageWizard.ImageFormats
{
    public class PngFormat : IImageFormat
    {
        public string MimeType => MimeTypes.Png;

        public void SaveImage(SKBitmap image, Stream stream)
        {
            image.Encode(stream, SKEncodedImageFormat.Png, 85);
        }
    }
}
