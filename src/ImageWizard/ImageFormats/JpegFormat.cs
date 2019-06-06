using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageWizard.ImageFormats
{
    public class JpegFormat : IImageFormat
    {
        public JpegFormat()
        {
            Quality = 85;
        }

        /// <summary>
        /// Quality
        /// </summary>
        public int Quality { get; set; }

        public string MimeType => "image/jpg";

        public void SaveImage(Image<Rgba32> image, Stream stream)
        {
            image.SaveAsJpeg(stream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder() { Quality = Quality });
        }
    }
}
