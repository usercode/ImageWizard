// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public partial class FlipFilter : ImageSharpFilter
{
    [Filter]
    public void Flip(Utils.FlipMode flippingMode)
    {
        var flipMode = flippingMode switch
        {
            Utils.FlipMode.Horizontal => FlipMode.Horizontal,
            Utils.FlipMode.Vertical => FlipMode.Vertical,
            _ => throw new Exception(),
        };

        Context.Image.Mutate(m => m.Flip(flipMode));
    }
}
