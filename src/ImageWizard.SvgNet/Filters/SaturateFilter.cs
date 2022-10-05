// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using Svg.FilterEffects;
using System.Globalization;

namespace ImageWizard.SvgNet.Filters;

public class SaturateFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Saturate(float value)
    {
        Context.Filters.Add(new SvgColourMatrix() { Type = SvgColourMatrixType.Saturate, 
                                    Values = value.ToString("0.0##", CultureInfo.InvariantCulture)
        });
    }
}
