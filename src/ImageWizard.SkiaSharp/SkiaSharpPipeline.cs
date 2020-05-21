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

        public override async Task StartAsync(ProcessingPipelineContext context)
        {
            IImageFormat targetFormat = ImageFormatHelper.Parse(context.CurrentImage.MimeType);

            SKBitmap bitmap = SKBitmap.Decode(context.CurrentImage.Data);

            SkiaSharpFilterContext filterContext = new SkiaSharpFilterContext(bitmap, targetFormat, context.ClientHints);

            //process filters
            while (context.UrlFilters.Count > 0)
            {
                string filter = context.UrlFilters.Peek();

                //find and execute filter
                IFilterAction foundFilter = FilterActions.FirstOrDefault(x => x.TryExecute(filter, filterContext));

                if (foundFilter != null)
                {
                    context.UrlFilters.Dequeue();
                }
                else
                {
                    throw new Exception($"filter was not found: {filter}");
                }
            }

            MemoryStream mem = new MemoryStream();
            filterContext.ImageFormat.SaveImage(filterContext.Image, mem);

            context.CurrentImage = new CurrentImage(
                                                mem.ToArray(),
                                                filterContext.ImageFormat.MimeType,
                                                filterContext.Image.Width,
                                                filterContext.Image.Height,
                                                filterContext.ClientHints.DPR);
        }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new SkiaSharpFilterAction<TFilter>(regex, methodInfo);
        }
    };
}
