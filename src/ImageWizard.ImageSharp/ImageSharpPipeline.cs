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
        public ImageSharpPipeline(IOptions<ImageSharpOptions> options, ILogger<ImageSharpPipeline> logger, IEnumerable<PipelineAction<ImageSharpPipeline>> actions)
            : base(logger)
        {
            Options = options.Value;
            Logger = logger;

            AddFilter<ResizeFilter>();
            AddFilter<BackgroundColorFilter>();
            AddFilter<CropFilter>();
            AddFilter<GrayscaleFilter>();
            AddFilter<BlackWhiteFilter>();
            AddFilter<TrimFilter>();
            AddFilter<FlipFilter>();
            AddFilter<RotateFilter>();
            AddFilter<BlurFilter>();
            //AddFilter<TextFilter>();
            AddFilter<InvertFilter>();
            AddFilter<BrightnessFilter>();
            AddFilter<ContrastFilter>();
            AddFilter<DPRFilter>();
            AddFilter<NoImageCacheFilter>();
            AddFilter<AutoOrientFilter>();
            AddFilter<ImageFormatFilter>();

            actions.Foreach(x => x(this));
        }

        /// <summary>
        /// Options
        /// </summary>
        public ImageSharpOptions Options { get; }

        public ILogger<ImageSharpPipeline> Logger { get; set; }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new ImageSharpFilterAction<TFilter>(regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(ProcessingPipelineContext context)
        {
            Image image = Image.Load(context.Result.Data);
            
            IImageFormat targetFormat = ImageFormatHelper.Parse(context.Result.MimeType);

            return new ImageSharpFilterContext(context, image, targetFormat, context.ClientHints);
        }
    }
}
