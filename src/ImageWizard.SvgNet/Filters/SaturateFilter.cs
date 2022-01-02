using ImageWizard.Attributes;
using ImageWizard.Filters;
using Svg;
using Svg.FilterEffects;
using Svg.Transforms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ImageWizard.SvgNet.Filters
{
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
}
