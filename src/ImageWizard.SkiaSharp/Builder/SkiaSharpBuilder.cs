using ImageWizard.Core.Processing.Builder;
using ImageWizard.SkiaSharp.Filters;
using ImageWizard.SkiaSharp.Filters.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp.Builder
{
    class SkiaSharpBuilder : PipelineBuilder, ISkiaSharpBuilder
    {
        public SkiaSharpBuilder(IServiceCollection services)
            :base(services)
        {
        }
    }
}
