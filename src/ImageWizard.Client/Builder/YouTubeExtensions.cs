using ImageWizard.Client.Builder.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// ImageBuilderYouTubeExtensions
    /// </summary>
    public static class YouTubeExtensions
    {
        public static IImageFilter Youtube(this ILoader imageUrlBuilder, string id)
        {
            return (IImageFilter)imageUrlBuilder.LoadData("youtube", id);
        }
    }
}
