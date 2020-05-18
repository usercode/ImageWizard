using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Middlewares;
using ImageWizard.Filters;
using ImageWizard.ImageSharp.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.ImageSharp.Builder
{
    public interface IImageSharpBuilder : IImageWizardBuilder
    {
        IImageSharpBuilder WithOptions(Action<ImageSharpOptions> action);

        IImageSharpBuilder WithFilter<TFilter>() where TFilter : ImageFilter, new();


    }
}
