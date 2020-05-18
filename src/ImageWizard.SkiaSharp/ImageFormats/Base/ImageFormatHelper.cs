﻿using ImageWizard.Core.Types;
using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    class ImageFormatHelper
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

                case MimeTypes.WebP:
                    imageFormat = new WebPFormat();
                    break;

                default:
                    throw new Exception();
            }

            return imageFormat;
        }
    }
}
