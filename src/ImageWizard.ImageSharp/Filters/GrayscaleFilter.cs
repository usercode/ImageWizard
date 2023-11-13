// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

public class GrayscaleFilter : ImageSharpFilter
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
