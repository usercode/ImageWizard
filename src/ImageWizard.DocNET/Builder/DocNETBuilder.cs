using ImageWizard.Core.Processing.Builder;
using ImageWizard.DocNET.Filters;
using ImageWizard.DocNET.Filters.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.DocNET.Builder
{
    class DocNETBuilder : PipelineBuilder, IDocNETBuilder
    {
        public DocNETBuilder(IServiceCollection services)
            : base(services)
        {
        }
    }
}
