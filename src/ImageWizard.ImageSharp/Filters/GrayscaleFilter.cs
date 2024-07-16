// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public partial class GrayscaleFilter : ImageSharpFilter
{
    [Filter]
    public void Grayscale()
    {
        Context.Image.Mutate(m => m.Grayscale());
    }

    [Filter]
    public void Grayscale(float amount)
    {
        Context.Image.Mutate(m => m.Grayscale(amount));
    }
}
