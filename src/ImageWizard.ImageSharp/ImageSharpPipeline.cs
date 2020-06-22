using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.ImageSharp.Builder;
using ImageWizard.Services.Types;
using ImageWizard.Utils.FilterTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    /// <summary>
    /// ImageSharpPipeline
    /// </summary>
    public class ImageSharpPipeline : ProcessingPipeline<ImageSharpFilter>
    {
        public ImageSharpPipeline(
            IServiceProvider service, 
            ILogger<ImageSharpPipeline> logger, 
            IEnumerable<PipelineAction<ImageSharpPipeline>> actions)
            : base(service, logger)
        {
            actions.Foreach(x => x(this));
        }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new ImageSharpFilterAction<TFilter>(ServiceProvider, regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(ProcessingPipelineContext context)
        {
            Image image = Image.Load(context.Result.Data);
            
            IImageFormat targetFormat = ImageFormatHelper.Parse(context.Result.MimeType);

            return new ImageSharpFilterContext(context, image, targetFormat, context.ClientHints);
        }
    }
}
