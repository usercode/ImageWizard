// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client
{
    /// <summary>
    /// ImageFilterFormatExtensions
    /// </summary>
    public static class ImageFilterFormatExtensions
    {
        public static IBuildUrl Jpg(this Image image)
        {
            return image.Filter("jpg()");
        }

        public static IBuildUrl Jpg(this Image image, int quality)
        {
            return image.Filter($"jpg({quality})");
        }

        public static IBuildUrl Png(this Image image)
        {
            return image.Filter("png()");
        }

        public static IBuildUrl Gif(this Image image)
        {
            return image.Filter("gif()");
        }

        public static IBuildUrl Bmp(this Image image)
        {
            return image.Filter("bmp()");
        }

        public static IBuildUrl WebP(this Image image)
        {
            return image.Filter("webp()");
        }
    }
}
