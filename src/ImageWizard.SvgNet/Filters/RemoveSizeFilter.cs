using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Filters;
using Svg;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SvgNet.Filters
{
    public class RemoveSizeFilter : SvgFilter
    {
        [Filter]
        public void RemoveSize()
        {
            Context.Image.Width = SvgUnit.None;
            Context.Image.Height = SvgUnit.None;
        }

    }
}
