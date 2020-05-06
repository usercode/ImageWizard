using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Filters;
using Svg;
using Svg.FilterEffects;
using Svg.Transforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SvgNet.Filters
{
    public class RotateFilter : ImageWizard.Filters.SvgFilter
    {
        [Filter]
        public void Rotate(float angle)
        {
            Context.Image.Transforms.Add(new SvgRotate(angle));
        }

    }
}
