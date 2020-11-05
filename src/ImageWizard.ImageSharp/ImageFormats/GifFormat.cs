using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageWizard.Core.Types;
using ImageWizard.ImageFormats.Base;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageWizard.ImageFormats
{
    public class GifFormat : IImageFormat
    {
        public string MimeType => MimeTypes.Gif;

        public async Task SaveImageAsync(Image image, Stream stream)
        {
            await image.SaveAsGifAsync(stream);
        }
    }
}
