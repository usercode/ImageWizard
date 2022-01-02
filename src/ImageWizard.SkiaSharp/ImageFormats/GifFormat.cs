using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageWizard.ImageFormats.Base;
using SkiaSharp;

namespace ImageWizard.ImageFormats
{
    public class GifFormat : IImageFormat
    {
        public string MimeType => MimeTypes.Gif;

        public void SaveImage(SKBitmap image, Stream stream)
        {
            image.Encode(stream, SKEncodedImageFormat.Gif, 85);
        }
    }
}
