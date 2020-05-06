using ImageWizard.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class MimeTypeHelper
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

                case ".svg":
                    mimeType = MimeTypes.Svg;
                    break;

                default:
                    throw new Exception("unknown file extension");
            }

            return mimeType;
        }
    }
}
