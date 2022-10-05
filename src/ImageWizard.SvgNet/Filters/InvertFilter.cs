// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using Svg.FilterEffects;

namespace ImageWizard.SvgNet.Filters;

public class InvertFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Invert()
    {
        Context.Filters.Add(new SvgColourMatrix() { Type = SvgColourMatrixType.Matrix, 
                                    Values = "-1 0 0 0 1 0 -1 0 0 1 0 0 -1 0 1 0 0 0 1 0 "
        });
    }

}
