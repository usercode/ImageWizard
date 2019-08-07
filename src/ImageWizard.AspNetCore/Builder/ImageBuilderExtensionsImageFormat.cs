using ImageWizard.AspNetCore.Builder;
using ImageWizard.AspNetCore.Builder.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore
{
    /// <summary>
    /// ImageBuilderExtensionsImageFormat
    /// </summary>
    public static class ImageBuilderExtensionsImageFormat
    {
        public static IImageBuildUrl Jpg(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter("jpg()");

            return imageUrlBuilder;
        }

        public static IImageBuildUrl Jpg(this IImageFilters imageUrlBuilder, int quality)
        {
            imageUrlBuilder.Filter($"jpg({quality})");

            return imageUrlBuilder;
        }

        public static IImageBuildUrl Png(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter("png()");

            return imageUrlBuilder;
        }

        public static IImageBuildUrl Gif(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter("gif()");

            return imageUrlBuilder;
        }

        public static IImageBuildUrl Bmp(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter("bmp()");

            return imageUrlBuilder;
        }
    }
}
