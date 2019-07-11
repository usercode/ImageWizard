using ImageWizard.Core.Types;
using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class ImageFormatHelper
    {
        public static IImageFormat Parse(string mimeType)
        {
            IImageFormat imageFormat;

            switch (mimeType)
            {
                case MimeTypes.Jpeg:
                    imageFormat = new JpegFormat();
                    break;

                case MimeTypes.Png:
                    imageFormat = new PngFormat();
                    break;

                case MimeTypes.Gif:
                    imageFormat = new GifFormat();
                    break;

                case MimeTypes.Bitmap:
                    imageFormat = new BmpFormat();
                    break;

                case MimeTypes.Svg:
                    imageFormat = new SvgFormat();
                    break;

                default:
                    throw new Exception();
            }

            return imageFormat;
        }
    }
}
