using ImageWizard.Client.Builder;
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
            imageUrlBuilder.Filter($"backgroundcolor({r.ToUrlString()},{g.ToUrlString()},{b.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilters BackgroundColor(this IImageFilters imageUrlBuilder, byte r, byte g, byte b)
        {
            imageUrlBuilder.Filter($"backgroundcolor({r},{g},{b})");

            return imageUrlBuilder;
        }

        public static IImageFilters DPR(this IImageFilters imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"dpr({value.ToUrlString()})");

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
            imageUrlBuilder.Filter($"crop({x.ToUrlString()},{y.ToUrlString()},{width.ToUrlString()},{heigth.ToUrlString()})");

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
            imageUrlBuilder.Filter($"resize({width},{height},{mode.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilters Resize(this IImageFilters imageUrlBuilder, int width, int height, ResizeMode mode, AnchorPositionMode anchorPosition)
        {
            imageUrlBuilder.Filter($"resize({width},{height},{mode.ToUrlString()},{anchorPosition.ToUrlString()})");

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

        public static IImageFilters Brightness(this IImageFilters imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"brightness({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilters Contrast(this IImageFilters imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"contrast({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilters Invert(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"invert()");

            return imageUrlBuilder;
        }

        public static IImageFilters Rotate(this IImageFilters imageUrlBuilder, RotateMode mode)
        {
            imageUrlBuilder.Filter($"rotate({(int)mode})");

            return imageUrlBuilder;
        }

        public static IImageFilters Rotate(this IImageFilters imageUrlBuilder, double angle)
        {
            imageUrlBuilder.Filter($"rotate({angle.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilters Flip(this IImageFilters imageUrlBuilder, FlipMode flippingMode)
        {
            imageUrlBuilder.Filter($"flip({flippingMode.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilters RoundedCorner(this IImageFilters imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"roundedcorner({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilters SvgRemoveSize(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter("removesize()");

            return imageUrlBuilder;
        }

        public static IImageFilters SvgRotate(this IImageFilters imageUrlBuilder, double angle)
        {
            imageUrlBuilder.Filter($"rotate({angle.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilters SvgBlur(this IImageFilters imageUrlBuilder)
        {
            imageUrlBuilder.Filter("blur()");

            return imageUrlBuilder;
        }
    }
}
