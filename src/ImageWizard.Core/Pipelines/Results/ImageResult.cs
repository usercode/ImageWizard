// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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
