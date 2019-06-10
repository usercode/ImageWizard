using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageWizard.ImageFormats.Base;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageWizard.ImageFormats
{
    public class PngFormat : IImageFormat
    {
        public string MimeType => "image/png";

        public void SaveImage(Image<Rgba32> image, Stream stream)
        {
            image.SaveAsPng(stream);
        }
    }
}
