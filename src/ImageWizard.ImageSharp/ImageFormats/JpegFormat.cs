using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageWizard.ImageFormats.Base;
using SixLabors.ImageSharp;

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

        /// <summary>
        /// MimeType
        /// </summary>
        public string MimeType => MimeTypes.Jpeg;

        public async Task SaveImageAsync(Image image, Stream stream)
        {
            await image.SaveAsJpegAsync(stream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder() { Quality = Quality });
        }
    }
}
