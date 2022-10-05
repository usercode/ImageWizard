// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client;

/// <summary>
/// ImageFilterExtensions
/// </summary>
public static class ImageFilterExtensions
{
    public static Image BackgroundColor(this Image image, double r, double g, double b)
    {
        image.Filter($"backgroundcolor({r.ToUrlString()},{g.ToUrlString()},{b.ToUrlString()})");

        return image;
    }

    public static Image BackgroundColor(this Image image, byte r, byte g, byte b)
    {
        image.Filter($"backgroundcolor({r},{g},{b})");

        return image;
    }

    public static Image DPR(this Image image, double value)
    {
        image.Filter($"dpr({value.ToUrlString()})");

        return image;
    }

    public static Image NoCache(this Image image)
    {
        image.Filter($"nocache()");

        return image;
    }

    public static Image Crop(this Image image, int width, int heigth)
    {
        Crop(image, 0, 0, width, heigth);

        return image;
    }

    public static Image Crop(this Image image, int x, int y, int width, int heigth)
    {
        image.Filter($"crop({x},{y},{width},{heigth})");

        return image;
    }

    public static Image Crop(this Image image, double width, double heigth)
    {
        Crop(image, 0, 0, width, heigth);

        return image;
    }

    public static Image Crop(this Image image, double x, double y, double width, double heigth)
    {
        image.Filter($"crop({x.ToUrlString()},{y.ToUrlString()},{width.ToUrlString()},{heigth.ToUrlString()})");

        return image;
    }

    public static Image Blur(this Image image)
    {
        image.Filter($"blur()");

        return image;
    }

    public static Image Blur(this Image image, int radius)
    {
        image.Filter($"blur({radius})");

        return image;
    }

    public static Image Resize(this Image image, int size)
    {
        image.Filter($"resize({size})");

        return image;
    }

    public static Image Resize(this Image image, int width, int height)
    {
        image.Filter($"resize({width},{height})");

        return image;
    }

    public static Image Resize(this Image image, int width, int height, ResizeMode mode)
    {
        image.Filter($"resize({width},{height},{mode.ToUrlString()})");

        return image;
    }

    public static Image Resize(this Image image, int width, int height, ResizeMode mode, AnchorPositionMode anchorPosition)
    {
        image.Filter($"resize({width},{height},{mode.ToUrlString()},{anchorPosition.ToUrlString()})");

        return image;
    }

    public static Image Trim(this Image image)
    {
        image.Filter("trim()");

        return image;
    }

    public static Image Grayscale(this Image image)
    {
        image.Filter($"grayscale()");

        return image;
    }

    public static Image Grayscale(this Image image, double value)
    {
        image.Filter($"grayscale({value.ToUrlString()})");

        return image;
    }

    public static Image BlackWhite(this Image image)
    {
        image.Filter($"blackwhite()");

        return image;
    }

    public static Image Brightness(this Image image, double value)
    {
        image.Filter($"brightness({value.ToUrlString()})");

        return image;
    }

    public static Image Contrast(this Image image, double value)
    {
        image.Filter($"contrast({value.ToUrlString()})");

        return image;
    }

    public static Image Invert(this Image image)
    {
        image.Filter($"invert()");

        return image;
    }

    public static Image Saturate(this Image image, double value)
    {
        image.Filter($"saturate({value.ToUrlString()})");

        return image;
    }

    public static Image Rotate(this Image image, double angle)
    {
        image.Filter($"rotate({angle.ToUrlString()})");

        return image;
    }

    public static Image Flip(this Image image, FlipMode flippingMode)
    {
        image.Filter($"flip({flippingMode.ToUrlString()})");

        return image;
    }

    public static Image DrawText(this Image image, string text, int? size = null, double? x = null, double? y = null, bool useBase64Url = true)
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

        image.Filter($"drawtext({string.Join(",", builder)})");

        return image;
    }
}
