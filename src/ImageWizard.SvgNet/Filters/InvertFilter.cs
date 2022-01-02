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
}
