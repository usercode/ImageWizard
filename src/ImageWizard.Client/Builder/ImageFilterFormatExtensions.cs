using ImageWizard.Client.Builder.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// ImageFilterFormatExtensions
    /// </summary>
    public static class ImageFilterFormatExtensions
    {
        public static IBuildUrl Jpg(this IImageFilter imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("jpg()");
        }

        public static IBuildUrl Jpg(this IImageFilter imageUrlBuilder, int quality)
        {
            return imageUrlBuilder.Filter($"jpg({quality})");
        }

        public static IBuildUrl Png(this IImageFilter imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("png()");
        }

        public static IBuildUrl Gif(this IImageFilter imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("gif()");
        }

        public static IBuildUrl Bmp(this IImageFilter imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("bmp()");
        }

        public static IBuildUrl WebP(this IImageFilter imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("webp()");
        }
    }
}
