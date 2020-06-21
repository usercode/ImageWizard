using ImageWizard.Core.Types;
using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class ImageFormatHelper
    {
        public static IImageFormat Parse(string mimeType)
        {
            IImageFormat imageFormat = mimeType switch
            {
                MimeTypes.Jpeg => new JpegFormat(),
                MimeTypes.Png => new PngFormat(),
                MimeTypes.Gif => new GifFormat(),
                MimeTypes.Bmp => new BmpFormat(),
                _ => throw new Exception(),
            };
            return imageFormat;
        }
    }
}
