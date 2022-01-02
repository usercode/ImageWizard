using ImageWizard.Core;
using ImageWizard.FFMpegCore.Filters.Base;
using ImageWizard.Processing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.FFMpegCore
{
    public class FFMpegCorePipeline : ProcessingPipeline<FFMpegCoreFilter>
    {
        public FFMpegCorePipeline(
            IServiceProvider serviceProvider, 
            ILogger<FFMpegCorePipeline> logger, 
            IEnumerable<PipelineAction<FFMpegCorePipeline>> actions)
            : base(serviceProvider, logger)
        {
            actions.Foreach(x => x(this));
        }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new FFMpegCoreFilterAction<TFilter>(ServiceProvider, regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(ProcessingPipelineContext context)
        {
            return new FFMpegCoreContext(context);
        }
    }
}
