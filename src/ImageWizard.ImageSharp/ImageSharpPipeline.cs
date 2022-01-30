using ImageWizard.Core;
using ImageWizard.Processing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    public class ImageSharpPipeline : Pipeline<ImageSharpFilter>
    {
        public ImageSharpPipeline(
            IServiceProvider service, 
            IOptions<ImageSharpOptions> options,
            ILogger<ImageSharpPipeline> logger, 
            IEnumerable<PipelineAction<ImageSharpPipeline>> actions)
            : base(service, logger)
        {
            Options = options;

            actions.Foreach(x => x(this));
        }

        /// <summary>
        /// Options
        /// </summary>
        private IOptions<ImageSharpOptions> Options { get; }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new ImageSharpFilterAction<TFilter>(ServiceProvider, regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(PipelineContext context)
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

            ImageSharpFilterContext imageSharpContext = new ImageSharpFilterContext(context, image, targetFormat, Options);

            //execute preprocessing            
            ImagePreProcessing preProcessing = ServiceProvider.GetService<ImagePreProcessing>();
            preProcessing?.Invoke(imageSharpContext);

            return imageSharpContext;
        }
    }
}
