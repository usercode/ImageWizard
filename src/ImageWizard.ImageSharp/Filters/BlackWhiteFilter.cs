// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public class BlackWhiteFilter : ImageSharpFilter
{
    [Filter]
    public void BlackWhite()
    {
        Context.Image.Mutate(m => m.BlackWhite());
    }
}
