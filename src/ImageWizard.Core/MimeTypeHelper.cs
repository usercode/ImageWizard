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
            string extension = Path.GetExtension(path).ToLower();
            string mimeType = extension switch
            {
                //image
                ".jpg" => MimeTypes.Jpeg,
                ".png" => MimeTypes.Png,
                ".gif" => MimeTypes.Gif,
                ".bmp" => MimeTypes.Bmp,
                ".webp" => MimeTypes.WebP,
                ".svg" => MimeTypes.Svg,

                //video
                ".mpeg" => MimeTypes.Mpeg,
                ".mpg" => MimeTypes.Mpeg,
                ".mpe" => MimeTypes.Mpeg,
                ".mp4" => MimeTypes.Mp4,
                ".ogg" => MimeTypes.Ogg,
                ".avi" => MimeTypes.Avi,
                ".webm" => MimeTypes.Webm,
                ".3gpp" => MimeTypes.Mobile3GP,

                _ => throw new Exception("unknown file extension"),
            };
            return mimeType;
        }
    }
}
