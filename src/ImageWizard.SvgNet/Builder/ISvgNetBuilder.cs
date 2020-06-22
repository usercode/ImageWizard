using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Middlewares;
using ImageWizard.Filters;
using ImageWizard.SvgNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SvgNet.Builder
{
    public interface ISvgNetBuilder : IImageWizardBuilder
    {
        ISvgNetBuilder WithOptions(Action<SvgOptions> action);

        ISvgNetBuilder WithFilter<TFilter>() where TFilter : SvgFilter, new();


    }
}
