using ImageWizard.Core.Processing.Builder;
using ImageWizard.Filters;
using ImageWizard.SvgNet.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SvgNet.Builder
{
    class SvgNetBuilder : PipelineBuilder, ISvgNetBuilder
    {
        public SvgNetBuilder(IServiceCollection service)
            : base(service)
        {
        }      
    }
}
