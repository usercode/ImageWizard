// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

public class BlurFilter : ImageSharpFilter
{
    [Filter]
    public void Blur()
    {
        Blur(10);
    }

    [Filter]
    public void Blur(int radius)
    {
        Context.Image.Mutate(m => m.BoxBlur(radius));
    }
}
