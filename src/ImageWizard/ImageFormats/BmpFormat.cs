using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageWizard.ImageFormats
{
    public class BmpFormat : IImageFormat
    {
        public string MimeType => "image/bmp";

        public void SaveImage(Image<Rgba32> image, Stream stream)
        {
            image.SaveAsBmp(stream);
        }
    }
}
