using ImageWizard.Core;
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
    public class SvgPipeline : Pipeline<SvgFilter, SvgFilterContext>
    {
        public SvgPipeline(
            IServiceProvider serviceProvider,
            ILogger<SvgPipeline> logger, 
            IEnumerable<PipelineAction<SvgPipeline>> actions)
            : base(serviceProvider, logger)
        {
            actions.Foreach(x => x(this));
        }

        protected override SvgFilterContext CreateFilterContext(PipelineContext context)
        {
            //load image
            SvgDocument svg = SvgDocument.Open<SvgDocument>(context.Result.Data);

            svg.Transforms = new SvgTransformCollection();

            return new SvgFilterContext(context, svg);
        }
    }
}
