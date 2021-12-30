using ImageWizard.Client.Builder;
using ImageWizard.Client.Builder.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;

namespace ImageWizard
{
    /// <summary>
    /// FilterExtensions
    /// </summary>
    public static class FilterExtensions
    {
        public static IImageFilter AsImage(this IFilter imageUrlBuilder)
        {
            return (IImageFilter)imageUrlBuilder;
        }

        public static ISvgFilter AsSvg(this IFilter imageUrlBuilder)
        {
            return (ISvgFilter)imageUrlBuilder;
        }

        public static IPdfFilter AsPdf(this IFilter imageUrlBuilder)
        {
            return (IPdfFilter)imageUrlBuilder;
        }
    }
}
