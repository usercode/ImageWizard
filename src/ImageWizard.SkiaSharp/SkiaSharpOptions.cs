﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.SkiaSharp;

/// <summary>
/// SkiaSharpOptions
/// </summary>
public class SkiaSharpOptions
{
    public SkiaSharpOptions()
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
