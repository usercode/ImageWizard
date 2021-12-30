using ImageWizard.Client.Builder;
using ImageWizard.Client.Builder.Types;
using ImageWizard.Utils;
using ImageWizard.Utils;
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
    /// ImageFilterExtensions
    /// </summary>
    public static class ImageFilterExtensions
    {
        public static IImageFilter BackgroundColor(this IImageFilter imageUrlBuilder, double r, double g, double b)
        {
            imageUrlBuilder.Filter($"backgroundcolor({r.ToUrlString()},{g.ToUrlString()},{b.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter BackgroundColor(this IImageFilter imageUrlBuilder, byte r, byte g, byte b)
        {
            imageUrlBuilder.Filter($"backgroundcolor({r},{g},{b})");

            return imageUrlBuilder;
        }

        public static IImageFilter DPR(this IImageFilter imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"dpr({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter NoCache(this IImageFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"nocache()");

            return imageUrlBuilder;
        }

        public static IImageFilter Crop(this IImageFilter imageUrlBuilder, int width, int heigth)
        {
            Crop(imageUrlBuilder, 0, 0, width, heigth);

            return imageUrlBuilder;
        }

        public static IImageFilter Crop(this IImageFilter imageUrlBuilder, int x, int y, int width, int heigth)
        {
            imageUrlBuilder.Filter($"crop({x},{y},{width},{heigth})");

            return imageUrlBuilder;
        }

        public static IImageFilter Crop(this IImageFilter imageUrlBuilder, double width, double heigth)
        {
            Crop(imageUrlBuilder, 0, 0, width, heigth);

            return imageUrlBuilder;
        }

        public static IImageFilter Crop(this IImageFilter imageUrlBuilder, double x, double y, double width, double heigth)
        {
            imageUrlBuilder.Filter($"crop({x.ToUrlString()},{y.ToUrlString()},{width.ToUrlString()},{heigth.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter Blur(this IImageFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"blur()");

            return imageUrlBuilder;
        }

        public static IImageFilter Blur(this IImageFilter imageUrlBuilder, int radius)
        {
            imageUrlBuilder.Filter($"blur({radius})");

            return imageUrlBuilder;
        }

        public static IImageFilter Resize(this IImageFilter imageUrlBuilder, int size)
        {
            imageUrlBuilder.Filter($"resize({size})");

            return imageUrlBuilder;
        }

        public static IImageFilter Resize(this IImageFilter imageUrlBuilder, int width, int height)
        {
            imageUrlBuilder.Filter($"resize({width},{height})");

            return imageUrlBuilder;
        }

        public static IImageFilter Resize(this IImageFilter imageUrlBuilder, int width, int height, ResizeMode mode)
        {
            imageUrlBuilder.Filter($"resize({width},{height},{mode.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter Resize(this IImageFilter imageUrlBuilder, int width, int height, ResizeMode mode, AnchorPositionMode anchorPosition)
        {
            imageUrlBuilder.Filter($"resize({width},{height},{mode.ToUrlString()},{anchorPosition.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter Trim(this IImageFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter("trim()");

            return imageUrlBuilder;
        }

        public static IImageFilter Grayscale(this IImageFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"grayscale()");

            return imageUrlBuilder;
        }

        public static IImageFilter Grayscale(this IImageFilter imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"grayscale({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter BlackWhite(this IImageFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"blackwhite()");

            return imageUrlBuilder;
        }

        public static IImageFilter Brightness(this IImageFilter imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"brightness({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter Contrast(this IImageFilter imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"contrast({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter Invert(this IImageFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"invert()");

            return imageUrlBuilder;
        }

        public static IImageFilter Saturate(this IImageFilter imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"saturate({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter Rotate(this IImageFilter imageUrlBuilder, double angle)
        {
            imageUrlBuilder.Filter($"rotate({angle.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter Flip(this IImageFilter imageUrlBuilder, FlipMode flippingMode)
        {
            imageUrlBuilder.Filter($"flip({flippingMode.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static IImageFilter PageToImage(this IImageFilter imageUrlBuilder, int pageIndex)
        {
            imageUrlBuilder.Filter($"pagetoimage({pageIndex})");

            return imageUrlBuilder;
        }

        public static IImageFilter PageToImage(this IImageFilter imageUrlBuilder, int pageIndex, int width, int height)
        {
            imageUrlBuilder.Filter($"pagetoimage({pageIndex},{width},{height})");

            return imageUrlBuilder;
        }

        public static IImageFilter DrawText(this IImageFilter imageUrlBuilder, string text, int? size = null, double? x = null, double? y = null, bool useBase64Url = true)
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

            imageUrlBuilder.Filter($"drawtext({string.Join(",", builder)})");

            return imageUrlBuilder;
        }
    }
}
