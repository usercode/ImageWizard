using ImageWizard.Client.Builder.Types;
using ImageWizard.Utils.FilterTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// ImageBuilderExtensionsEffects
    /// </summary>
    public static class ImageBuilderExtensionsEffects
    {
        public static IImageFilters BackgroundColor(this IImageFilters imageUrlBuilder, double r, double g, double b)
        {
            imageUrlBuilder.Filter($"backgroundcolor({r.ToString("0.0", CultureInfo.InvariantCulture)},{g.ToString("0.0", CultureInfo.InvariantCulture)},{b.ToString("0.0", CultureInfo.InvariantCulture)})");

            return imageUrlBuilder;
        }

        public static IImageFilters BackgroundColor(this IImageFilters imageUrlBuilder, byte r, byte g, byte b)
        {
            imageUrlBuilder.Filter($"backgroundcolor({r},{g},{b})");

            return imageUrlBuilder;
        }

        public static IImageFilters DPR(this IImageFilters imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"dpr({value.ToString("0.0", CultureInfo.InvariantCulture)})");

            return imageUrlBuilder;
        }

        public static IImageFilters NoCache(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"nocache()");

            return imageUrlBuilder;
        }

        public static IImageFilters Crop(this IImageFilters imageUrlBuilder, int width, int heigth)
        {
            Crop(imageUrlBuilder, 0, 0, width, heigth);

            return imageUrlBuilder;
        }

        public static IImageFilters Crop(this IImageFilters imageUrlBuilder, int x, int y, int width, int heigth)
        {
            imageUrlBuilder.Filter($"crop({x},{y},{width},{heigth})");

            return imageUrlBuilder;
        }

        public static IImageFilters Crop(this IImageFilters imageUrlBuilder, double width, double heigth)
        {
            Crop(imageUrlBuilder, 0, 0, width, heigth);

            return imageUrlBuilder;
        }

        public static IImageFilters Crop(this IImageFilters imageUrlBuilder, double x, double y, double width, double heigth)
        {
            imageUrlBuilder.Filter($"crop({x.ToString("0.0", CultureInfo.InvariantCulture)},{y.ToString("0.0", CultureInfo.InvariantCulture)},{width.ToString("0.0", CultureInfo.InvariantCulture)},{heigth.ToString("0.0", CultureInfo.InvariantCulture)})");

            return imageUrlBuilder;
        }
        public static IImageFilters Blur(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"blur()");

            return imageUrlBuilder;
        }

        public static IImageFilters Resize(this IImageFilters imageUrlBuilder, int size)
        {
            imageUrlBuilder.Filter($"resize({size})");

            return imageUrlBuilder;
        }

        public static IImageFilters Resize(this IImageFilters imageUrlBuilder, int width, int height)
        {
            imageUrlBuilder.Filter($"resize({width},{height})");

            return imageUrlBuilder;
        }

        public static IImageFilters Resize(this IImageFilters imageUrlBuilder, int width, int height, ResizeMode mode)
        {
            imageUrlBuilder.Filter($"resize({width},{height},{mode.ToString().ToLower()})");

            return imageUrlBuilder;
        }

        public static IImageFilters Resize(this IImageFilters imageUrlBuilder, int width, int height, ResizeMode mode, AnchorPositionMode anchorPosition)
        {
            imageUrlBuilder.Filter($"resize({width},{height},{mode.ToString().ToLower()},{anchorPosition.ToString().ToLower()})");

            return imageUrlBuilder;
        }

        public static IImageFilters Trim(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter("trim()");

            return imageUrlBuilder;
        }

        public static IImageFilters Grayscale(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"grayscale()");

            return imageUrlBuilder;
        }

        public static IImageFilters BlackWhite(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"blackwhite()");

            return imageUrlBuilder;
        }

        public static IImageFilters Rotate(this IImageFilters imageUrlBuilder, RotateMode mode)
        {
            imageUrlBuilder.Filter($"rotate({(int)mode})");

            return imageUrlBuilder;
        }

        public static IImageFilters Flip(this IImageFilters imageUrlBuilder, FlipMode flippingMode)
        {
            imageUrlBuilder.Filter($"flip({flippingMode.ToString().ToLower()})");

            return imageUrlBuilder;
        }

        public static IImageFilters RoundedCorner(this IImageFilters imageUrlBuilder, float value)
        {
            imageUrlBuilder.Filter($"roundedcorner({value.ToString("0.0", CultureInfo.InvariantCulture)})");

            return imageUrlBuilder;
        }

        public static IImageFilters SvgRemoveSize(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter("removesize()");

            return imageUrlBuilder;
        }

        public static IImageFilters SvgRotate(this IImageFilters imageUrlBuilder, float angle)
        {
            imageUrlBuilder.Filter($"rotate({angle.ToString("0.0", CultureInfo.InvariantCulture)})");

            return imageUrlBuilder;
        }

        public static IImageFilters SvgBlur(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter("blur()");

            return imageUrlBuilder;
        }
    }
}
