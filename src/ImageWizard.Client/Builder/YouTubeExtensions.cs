﻿using ImageWizard.Client.Builder.Types;
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
        public static Image Youtube(this ILoader imageUrlBuilder, string id)
        {
            return new Image(imageUrlBuilder.LoadData("youtube", id));
        }
    }
}
