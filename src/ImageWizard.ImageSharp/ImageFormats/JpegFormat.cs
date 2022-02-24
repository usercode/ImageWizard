// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ImageWizard.ImageSharp
{
    public class JpegFormat : IImageFormat
    {
        public JpegFormat()
        {
            Quality = 80;
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
            await image.SaveAsJpegAsync(stream, new JpegEncoder() { Quality = Quality });
        }
    }
}
