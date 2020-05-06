using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Core.ImageFilters.Base.Helpers;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Types;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.Services.Types;
using ImageWizard.SharedContract.FilterTypes;
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
        public SvgPipeline(ILogger<SvgPipeline> logger)
        {
            Logger = logger;

            AddFilter<RemoveSizeFilter>();
            AddFilter<RotateFilter>();
            AddFilter<BlurFilter>();
        }

        public override IEnumerable<string> MimeType => new[] { MimeTypes.Svg };

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

        public override async Task StartAsync(ProcessingPipelineContext context)
        {
            //load image
            SvgDocument svg = SvgDocument.Open<SvgDocument>(new MemoryStream(context.CurrentImage.Data));

            svg.Transforms = new SvgTransformCollection();

            SvgFilterContext filterContext = new SvgFilterContext(context.ImageWizardOptions, svg);

            //process filters
            bool resultFilters = ProcessFilters(context.UrlFilters, filterContext);

            if (resultFilters == false)
            {
                throw new Exception();
            }

            //apply filters
            if (filterContext.Filters.Any())
            {
                var defs = svg.Children.GetSvgElementOf<SvgDefinitionList>();

                if (defs == null)
                {
                    defs = new SvgDefinitionList();
                    svg.Children.Add(defs);
                }

                var filterElement = new Svg.FilterEffects.SvgFilter();
                filterElement.ID = "filter01";

                defs.Children.Add(filterElement);

                foreach (var f in filterContext.Filters)
                {
                    filterElement.Children.Add(f);
                }

                svg.CustomAttributes.Add("filter", $"url(#{filterElement.ID})");
            }

            MemoryStream mem = new MemoryStream();
            svg.Write(mem);

            context.CurrentImage = new CurrentImage(MimeTypes.Svg, mem.ToArray());
        }
    }
}
