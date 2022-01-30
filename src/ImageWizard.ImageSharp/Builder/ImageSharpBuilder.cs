using ImageWizard.Core.Processing.Builder;
using ImageWizard.ImageSharp.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    class ImageSharpBuilder : PipelineBuilder, IImageSharpBuilder
    {
        public ImageSharpBuilder(IServiceCollection services)
            : base(services)
        {
        }
    }
}
