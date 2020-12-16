using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.Services.Types;
using ImageWizard.SkiaSharp.Filters;
using ImageWizard.SkiaSharp.Filters.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.SkiaSharp
{
    /// <summary>
    /// SkiaPipeline
    /// </summary>
    public class SkiaSharpPipeline : ProcessingPipeline<SkiaSharpFilter>
    {
        public SkiaSharpPipeline(
            IServiceProvider serviceProvider, 
            ILogger<SkiaSharpPipeline> logger, 
            IEnumerable<PipelineAction<SkiaSharpPipeline>> actions)
            : base(serviceProvider, logger)
        {
            actions.Foreach(x => x(this));
        }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new SkiaSharpFilterAction<TFilter>(ServiceProvider, regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(ProcessingPipelineContext context)
        {
            IImageFormat targetFormat = null;

            if (context.ImageWizardOptions.UseAcceptHeader)
            {
                //try to use mime type from accept header
                targetFormat = ImageFormatHelper.FirstOrDefault(context.AcceptMimeTypes);
            }

            if(targetFormat == null)
            {
                //use mime type of the original image
                targetFormat = ImageFormatHelper.FirstOrDefault(context.Result.MimeType);
            }

            SKBitmap bitmap = SKBitmap.Decode(context.Result.Data);

            return new SkiaSharpFilterContext(context, bitmap, targetFormat);
        }
    }
}
