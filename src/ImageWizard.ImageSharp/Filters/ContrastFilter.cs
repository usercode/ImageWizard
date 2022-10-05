// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public class ContrastFilter : ImageSharpFilter
{
    [Filter]
    public void Contrast(float value)
    {
        Context.Image.Mutate(m => m.Contrast(value));
    }
}
