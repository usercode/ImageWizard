// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard.Client;

/// <summary>
/// VideoFilterExtensions
/// </summary>
public static class VideoFilterExtensions
{
    public static Video GetFrame(this Video video)
    {
        video.Filter($"frame()");

        return video;
    }
   
}
