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
    public class BmpFormat : IImageFormat
    {
        public string MimeType => MimeTypes.Bitmap;

        public void SaveImage(SKBitmap image, Stream stream)
        {
            image.Encode(stream, SKEncodedImageFormat.Bmp, 85);
        }
    }
}
