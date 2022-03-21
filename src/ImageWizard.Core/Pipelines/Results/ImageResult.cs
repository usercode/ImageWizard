// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Processing.Results;

/// <summary>
/// ImageResult
/// </summary>
public class ImageResult : DataResult
{
    public ImageResult(Stream data, string mimeType, int? width, int? height)
        : base(data, mimeType)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Width
    /// </summary>
    public int? Width { get; }

    /// <summary>
    /// Height
    /// </summary>
    public int? Height { get; }
}
