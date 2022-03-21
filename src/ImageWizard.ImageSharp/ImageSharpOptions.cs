// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.ImageSharp;

/// <summary>
/// ImageSharpOptions
/// </summary>
public class ImageSharpOptions
{
    public ImageSharpOptions()
    {
        ImageMaxWidth = 4000;
        ImageMaxHeight = 4000;
    }

    /// <summary>
    /// ImageMaxWidth
    /// </summary>
    public int? ImageMaxWidth { get; set; }

    /// <summary>
    /// ImageMaxHeight
    /// </summary>
    public int? ImageMaxHeight { get; set; }
}
