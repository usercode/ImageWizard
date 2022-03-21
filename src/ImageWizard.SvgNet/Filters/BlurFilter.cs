// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.Filters;
using Svg;
using Svg.FilterEffects;
using Svg.Transforms;
using System;
using System.Collections.Generic;
using System.Text;

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
