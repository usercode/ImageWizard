// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using Svg.FilterEffects;

namespace ImageWizard.SvgNet.Filters;

public class GrayscaleFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Grayscale()
    {
        Context.Filters.Add(new SvgColourMatrix() { Type = SvgColourMatrixType.Matrix, 
                                    Values =
                                    @"0.21 0.72 0.07 0 0
                                          0.21 0.72 0.07 0 0
                                          0.21 0.72 0.07 0 0
                                          0 0 0 1 0"
        });
    }

}
