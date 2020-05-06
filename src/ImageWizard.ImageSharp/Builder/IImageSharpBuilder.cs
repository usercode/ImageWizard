using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Filters;
using ImageWizard.ImageSharp.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.ImageSharp.Builder
{
    public interface IImageSharpBuilder
    {
        IImageSharpBuilder AddFilter<TFilter>() where TFilter : ImageFilter, new();


    }
}
