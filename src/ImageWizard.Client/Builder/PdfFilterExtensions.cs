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
    /// PdfFilterExtensions
    /// </summary>
    public static class PdfFilterExtensions
    {
        public static IImageFilter PageToImage(this IPdfFilter imageUrlBuilder, int pageIndex)
        {
            return (IImageFilter)imageUrlBuilder.Filter($"pagetoimage({pageIndex})");
        }

        public static IImageFilter PageToImage(this IPdfFilter imageUrlBuilder, int pageIndex, int width, int height)
        {
            return (IImageFilter)imageUrlBuilder.Filter($"pagetoimage({pageIndex},{width},{height})");
        }
    }
}
