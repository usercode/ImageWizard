// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public enum WatermarkLocation
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}
