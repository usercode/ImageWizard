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

        public static string GetMimeTypeByExtension(string path)
        {
            string mimeType;

            string extension = Path.GetExtension(path).ToLower();

            switch (extension)
            {
                case ".jpg":
                    mimeType = MimeTypes.Jpeg;
                    break;

                case ".png":
                    mimeType = MimeTypes.Png;
                    break;

                case ".gif":
                    mimeType = MimeTypes.Gif;
                    break;

                case ".bmp":
                    mimeType = MimeTypes.Bitmap;
                    break;

                default:
                    throw new Exception("unknown file extension");
            }

            return mimeType;
        }

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
