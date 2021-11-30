using ImageWizard.Client.Builder.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// ImageBuilderExtensionsImageFormat
    /// </summary>
    public static class ImageBuilderExtensionsImageFormat
    {
        public static IImageBuildUrl Jpg(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("jpg()");
        }

        public static IImageBuildUrl Jpg(this IImageFilters imageUrlBuilder, int quality)
        {
            return imageUrlBuilder.Filter($"jpg({quality})");
        }

        public static IImageBuildUrl Png(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("png()");
        }

        public static IImageBuildUrl Gif(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("gif()");
        }

        public static IImageBuildUrl Bmp(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("bmp()");
        }

        public static IImageBuildUrl WebP(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("webp()");
        }
    }
}
