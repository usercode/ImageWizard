// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard.Client;

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
