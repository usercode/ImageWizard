// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public partial class AutoOrientFilter : ImageSharpFilter
{
    [Filter]
    public void AutoOrient()
    {
        Context.Image.Mutate(m => m.AutoOrient());
    }
}
