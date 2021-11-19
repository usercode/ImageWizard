using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.Filters;
using ImageWizard.OpenCvSharp.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageWizard.ImageSharp.Filters
{
    /// <summary>
    /// ImageSharpPipeline
    /// </summary>
    public class OpenCvSharpPipeline : ProcessingPipeline<OpenCvSharpFilter>
    {
        public OpenCvSharpPipeline(
            IServiceProvider service, 
            ILogger<OpenCvSharpPipeline> logger, 
            IEnumerable<PipelineAction<OpenCvSharpPipeline>> actions)
            : base(service, logger)
        {
            actions.Foreach(x => x(this));
        }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new OpenCvSharpFilterAction<TFilter>(ServiceProvider, regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(ProcessingPipelineContext context)
        {
            

            return new OpenCvSharpFilterContext(context);
        }
    }
}
