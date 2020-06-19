using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Core.ImageFilters.Base.Helpers;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.Services.Types;
using Microsoft.AspNetCore.Components;
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
        public SvgPipeline(ILogger<SvgPipeline> logger, IEnumerable<PipelineAction<SvgPipeline>> actions)
            : base(logger)
        {
            Logger = logger;

            AddFilter<RemoveSizeFilter>();
            AddFilter<RotateFilter>();
            AddFilter<BlurFilter>();
            AddFilter<GrayscaleFilter>();
            AddFilter<InvertFilter>();
            AddFilter<SaturateFilter>();
            AddFilter<ImageFormatFilter>();

            actions.Foreach(x => x(this));
        }

        /// <summary>
        /// Logger
        /// </summary>
        public ILogger<SvgPipeline> Logger { get; set; }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new SvgFilterAction<TFilter>(
                                                    regex,
                                                    methodInfo);
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
