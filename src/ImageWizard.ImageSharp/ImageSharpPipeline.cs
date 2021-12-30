using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Settings;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.Processing;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

            IImageFormat targetFormat = null;

            if (context.ImageWizardOptions.UseAcceptHeader)
            {
                targetFormat = ImageFormatHelper.FirstOrDefault(context.AcceptMimeTypes);
            }

            if (targetFormat == null)
            {
                targetFormat = ImageFormatHelper.FirstOrDefault(context.Result.MimeType);
            }

            return new ImageSharpFilterContext(context, image, targetFormat);
        }
    }
}
