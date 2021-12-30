using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// MimeTypes
    /// </summary>
    public static class MimeTypes
    {
        public const string Object = "application/octet-stream";

        public const string Jpeg = "image/jpeg";
        public const string Png = "image/png";
        public const string Gif = "image/gif";
        public const string Bmp = "image/bmp";
        public const string WebP = "image/webp";
        public const string Avif = "image/avif";
        public const string Svg = "image/svg+xml";
        
        public const string Mpeg = "video/mpeg";
        public const string Mp4 = "video/mp4";
        public const string Ogg = "video/ogg";
        public const string Webm = "video/webm";
        public const string Avi = "video/x-msvideo";
        public const string Mobile3GP = "video/3gpp";

        public const string Pdf = "application/pdf";

        public const string Html = "text/html";
        public const string Css = "text/css";
        public const string Xml = "text/xml";

        public static string[] GetVideoMimeTypes()
        {
            return new [] { Mpeg, Mp4, Ogg,Webm, Avi, Mobile3GP };
        }

        public static string[] GetImageMimeTypes()
        {
            return new [] { Jpeg, Png, Gif, Bmp, WebP, Avif, Svg };
        }

        public static string GetByExtension(string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            string mimeType = extension switch
            {
                //image
                ".jpg" => Jpeg,
                ".png" => Png,
                ".gif" => Gif,
                ".bmp" => Bmp,
                ".webp" => WebP,
                ".avif" => Avif,
                ".svg" => Svg,

                //video
                ".mpeg" => Mpeg,
                ".mpg" => Mpeg,
                ".mpe" => Mpeg,
                ".mp4" => Mp4,
                ".ogg" => Ogg,
                ".avi" => Avi,
                ".webm" => Webm,
                ".3gpp" => Mobile3GP,

                _ => throw new Exception($"Unknown file extension: {extension}"),
            };
            return mimeType;
        }
    }
}
