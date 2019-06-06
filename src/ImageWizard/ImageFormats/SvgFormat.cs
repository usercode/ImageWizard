using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageWizard.ImageFormats
{
    public class SvgFormat : IImageFormat
    {
        public string MimeType => "image/svg+xml";

        public void SaveImage(Image<Rgba32> image, Stream stream)
        {
            //do nothing....
        }
    }
}
