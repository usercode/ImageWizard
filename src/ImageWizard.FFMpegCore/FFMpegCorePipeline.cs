using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.FFMpegCore.Filters.Base;
using ImageWizard.Filters;
using ImageWizard.Services.Types;
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
        public FFMpegCorePipeline(ILogger<FFMpegCorePipeline> logger, IEnumerable<PipelineAction<FFMpegCorePipeline>> actions)
            : base(logger)
        {
            actions.Foreach(x => x(this));
        }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new FFMpegCoreFilterAction<TFilter>(regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(ProcessingPipelineContext context)
        {
            return new FFMpegCoreContext(context);
        }
    }
}
