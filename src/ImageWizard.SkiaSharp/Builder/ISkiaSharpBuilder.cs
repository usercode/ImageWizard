using ImageWizard.Filters;
using ImageWizard.SkiaSharp.Filters.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp.Builder
{
    public interface ISkiaSharpBuilder : IImageWizardBuilder
    {
        ISkiaSharpBuilder WithOptions(Action<SkiaSharpOptions> action);

        ISkiaSharpBuilder WithFilter<TFilter>() where TFilter : SkiaSharpFilter, new();


    }
}
