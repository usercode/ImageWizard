// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using Svg;
using Svg.FilterEffects;

namespace ImageWizard.SvgNet.Filters;

public class BlurFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Blur()
    {
        Blur(5);
    }

    [Filter]
    public void Blur(float deviation)
    {
        var l = new SvgNumberCollection();
        l.Add(deviation);

        Context.Filters.Add(new SvgGaussianBlur() { StdDeviation = l });
    }
}
