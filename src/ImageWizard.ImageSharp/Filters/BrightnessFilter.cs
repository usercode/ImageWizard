// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public partial class BrightnessFilter : ImageSharpFilter
{
    [Filter]
    public void Brightness(float value)
    {
        Context.Image.Mutate(m => m.Brightness(value));
    }
}
