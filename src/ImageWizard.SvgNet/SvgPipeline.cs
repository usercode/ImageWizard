using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Settings;
using ImageWizard.Filters;
using ImageWizard.Processing;
using Microsoft.Extensions.Logging;
using Svg;
using Svg.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.SvgNet.Filters
{
    /// <summary>
    /// ImageSharpPipeline
    /// </summary>
    public class SvgPipeline : ProcessingPipeline<SvgFilter>
    {
        public SvgPipeline(
            IServiceProvider serviceProvider, 
            ILogger<SvgPipeline> logger, 
            IEnumerable<PipelineAction<SvgPipeline>> actions)
            : base(serviceProvider, logger)
        {

            actions.Foreach(x => x(this));
        }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new SvgFilterAction<TFilter>(ServiceProvider, regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(ProcessingPipelineContext context)
        {
            //load image
            SvgDocument svg = SvgDocument.Open<SvgDocument>(new MemoryStream(context.Result.Data));

            svg.Transforms = new SvgTransformCollection();

            return new SvgFilterContext(context, svg);
        }
    }
}
