using ImageWizard.Client.Builder;
using ImageWizard.Client.Builder.Types;
using ImageWizard.Utils.FilterTypes;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            return imageUrlBuilder.Filter($"backgroundcolor({r.ToUrlString()},{g.ToUrlString()},{b.ToUrlString()})");
        }

        public static IImageFilters BackgroundColor(this IImageFilters imageUrlBuilder, byte r, byte g, byte b)
        {
            return imageUrlBuilder.Filter($"backgroundcolor({r},{g},{b})");
        }

        public static IImageFilters DPR(this IImageFilters imageUrlBuilder, double value)
        {
            return imageUrlBuilder.Filter($"dpr({value.ToUrlString()})");
        }

        public static IImageFilters NoCache(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter($"nocache()");
        }

        public static IImageFilters Crop(this IImageFilters imageUrlBuilder, int width, int heigth)
        {
            Crop(imageUrlBuilder, 0, 0, width, heigth);

            return imageUrlBuilder;
        }

        public static IImageFilters Crop(this IImageFilters imageUrlBuilder, int x, int y, int width, int heigth)
        {
            return imageUrlBuilder.Filter($"crop({x},{y},{width},{heigth})");
        }

        public static IImageFilters Crop(this IImageFilters imageUrlBuilder, double width, double heigth)
        {
            return Crop(imageUrlBuilder, 0, 0, width, heigth);
        }

        public static IImageFilters Crop(this IImageFilters imageUrlBuilder, double x, double y, double width, double heigth)
        {
            return imageUrlBuilder.Filter($"crop({x.ToUrlString()},{y.ToUrlString()},{width.ToUrlString()},{heigth.ToUrlString()})");
        }
        public static IImageFilters Blur(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter($"blur()");
        }

        public static IImageFilters Blur(this IImageFilters imageUrlBuilder, int radius)
        {
            return imageUrlBuilder.Filter($"blur({radius})");
        }

        public static IImageFilters Resize(this IImageFilters imageUrlBuilder, int size)
        {
            return imageUrlBuilder.Filter($"resize({size})");
        }

        public static IImageFilters Resize(this IImageFilters imageUrlBuilder, int width, int height)
        {
            return imageUrlBuilder.Filter($"resize({width},{height})");
        }

        public static IImageFilters Resize(this IImageFilters imageUrlBuilder, int width, int height, ResizeMode mode)
        {
            return imageUrlBuilder.Filter($"resize({width},{height},{mode.ToUrlString()})");
        }

        public static IImageFilters Resize(this IImageFilters imageUrlBuilder, int width, int height, ResizeMode mode, AnchorPositionMode anchorPosition)
        {
            return imageUrlBuilder.Filter($"resize({width},{height},{mode.ToUrlString()},{anchorPosition.ToUrlString()})");
        }

        public static IImageFilters Trim(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("trim()");
        }

        public static IImageFilters Grayscale(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter($"grayscale()");
        }

        public static IImageFilters Grayscale(this IImageFilters imageUrlBuilder, double value)
        {
            return imageUrlBuilder.Filter($"grayscale({value.ToUrlString()})");
        }

        public static IImageFilters BlackWhite(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter($"blackwhite()");
        }

        public static IImageFilters Brightness(this IImageFilters imageUrlBuilder, double value)
        {
            return imageUrlBuilder.Filter($"brightness({value.ToUrlString()})");
        }

        public static IImageFilters Contrast(this IImageFilters imageUrlBuilder, double value)
        {
            return imageUrlBuilder.Filter($"contrast({value.ToUrlString()})");
        }

        public static IImageFilters Invert(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter($"invert()");
        }

        public static IImageFilters Saturate(this IImageFilters imageUrlBuilder, double value)
        {
            return imageUrlBuilder.Filter($"saturate({value.ToUrlString()})");
        }

        public static IImageFilters Rotate(this IImageFilters imageUrlBuilder, double angle)
        {
            return imageUrlBuilder.Filter($"rotate({angle.ToUrlString()})");
        }

        public static IImageFilters Flip(this IImageFilters imageUrlBuilder, FlipMode flippingMode)
        {
            return imageUrlBuilder.Filter($"flip({flippingMode.ToUrlString()})");
        }

        public static IImageFilters SvgRemoveSize(this IImageFilters imageUrlBuilder)
        {
            return imageUrlBuilder.Filter("removesize()");
        }

        public static IImageFilters PageToImage(this IImageFilters imageUrlBuilder, int pageIndex)
        {
            return imageUrlBuilder.Filter($"pagetoimage({pageIndex})");
        }

        public static IImageFilters PageToImage(this IImageFilters imageUrlBuilder, int pageIndex, int width, int height)
        {
            return imageUrlBuilder.Filter($"pagetoimage({pageIndex},{width},{height})");
        }

        public static IImageFilters DrawText(this IImageFilters imageUrlBuilder, string text, int? size = null, double? x = null, double? y = null, bool useBase64Url = true)
        {
            List<string> builder = new List<string>();

            if (useBase64Url)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                string base64Url = WebEncoders.Base64UrlEncode(buffer);

                builder.Add($"text={base64Url}");
            }
            else
            {
                builder.Add($"text='{text}'");
            }

            if (size != null)
            {
                builder.Add($"size={size}");
            }

            if (x != null)
            {
                builder.Add($"x={x.Value.ToUrlString()}");
            }

            if (y != null)
            {
                builder.Add($"y={y.Value.ToUrlString()}");
            }

            return imageUrlBuilder.Filter($"drawtext({string.Join(",", builder)})");
        }
    }
}
