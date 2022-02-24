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

namespace ImageWizard.SvgNet.Filters
{
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
}
