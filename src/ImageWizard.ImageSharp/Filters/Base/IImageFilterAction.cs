using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.ImageSharp.Filters
{
    public interface IImageFilterAction : IFilterAction<ImageFilterContext>
    {
    }
}
