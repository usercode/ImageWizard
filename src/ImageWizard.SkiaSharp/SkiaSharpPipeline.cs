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
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
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
        public SkiaSharpPipeline(ILogger<SkiaSharpPipeline> logger, IEnumerable<PipelineAction<SkiaSharpPipeline>> actions)
            : base(logger)
        {
            Logger = logger;

            AddFilter<ResizeFilter>();
            AddFilter<RotateFilter>();
            AddFilter<CropFilter>();
            AddFilter<GrayscaleFilter>();
            AddFilter<BlurFilter>();
            AddFilter<FlipFilter>();
            AddFilter<DPRFilter>();
            AddFilter<ImageFormatFilter>();

            actions.Foreach(x => x(this));
        }

        /// <summary>
        /// Logger
        /// </summary>
        public ILogger<SkiaSharpPipeline> Logger { get; }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new SkiaSharpFilterAction<TFilter>(regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(ProcessingPipelineContext context)
        {
            IImageFormat targetFormat = ImageFormatHelper.Parse(context.Result.MimeType);

            SKBitmap bitmap = SKBitmap.Decode(context.Result.Data);

            return new SkiaSharpFilterContext(context, bitmap, targetFormat, context.ClientHints);
        }
    }
}
