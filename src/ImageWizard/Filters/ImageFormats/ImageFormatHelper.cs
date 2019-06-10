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
                case "image/jpeg":
                    imageFormat = new JpegFormat();
                    break;

                case "image/png":
                    imageFormat = new PngFormat();
                    break;

                case "image/gif":
                    imageFormat = new GifFormat();
                    break;

                case "image/bmp":
                    imageFormat = new BmpFormat();
                    break;

                case "image/svg+xml":
                    imageFormat = new SvgFormat();
                    break;

                default:
                    throw new Exception();
            }

            return imageFormat;
        }
    }
}
