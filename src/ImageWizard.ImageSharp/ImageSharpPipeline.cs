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
    public class ImageSharpPipeline : ProcessingPipeline<ImageFilter>
    {
        public ImageSharpPipeline(IOptions<ImageSharpOptions> options, ILogger<ImageSharpPipeline> logger, IEnumerable<PipelineAction<ImageSharpPipeline>> actions)
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
            return new ImageFilterAction<TFilter>(
                                                    regex,
                                                    methodInfo);
        }

        public override async Task StartAsync(ProcessingPipelineContext context)
        {
            //load image
            using (Image image = Image.Load(context.CurrentImage.Data))
            {
                IImageFormat targetFormat = ImageFormatHelper.Parse(context.CurrentImage.MimeType);

                ImageFilterContext filterContext = new ImageFilterContext(image, targetFormat, context.ClientHints);

                //process filters
                while(context.UrlFilters.Count > 0)
                {
                    string filter = context.UrlFilters.Peek();

                    //find and execute filter
                    IFilterAction foundFilter = FilterActions.FirstOrDefault(x => x.TryExecute(filter, filterContext));

                    if (foundFilter != null)
                    {
                        Logger.LogTrace("Filter executed: " + filter);

                        context.UrlFilters.Dequeue();
                    }
                    else
                    {
                        Logger.LogTrace($"filter was not found: {filter}");

                        throw new Exception($"filter was not found: {filter}");
                    }
                }

                //check max width and height
                bool change = false;

                int width = image.Width;
                int height = image.Height;

                if (Options.ImageMaxWidth != null && width > Options.ImageMaxWidth)
                {
                    change = true;
                    width = Options.ImageMaxWidth.Value;
                }

                if (Options.ImageMaxHeight != null && height > Options.ImageMaxHeight)
                {
                    change = true;
                    height = Options.ImageMaxHeight.Value;
                }

                if (change == true)
                {
                    new ResizeFilter() { Context = filterContext }.Resize(width, height, ResizeMode.Max);
                }

                MemoryStream mem = new MemoryStream();

                //save image
                filterContext.ImageFormat.SaveImage(image, mem);

                byte[] transformedImageData = mem.ToArray();

                //update some metadata
                context.DisableCache = filterContext.NoImageCache;
                context.CurrentImage = new CurrentImage(
                                                transformedImageData,
                                                filterContext.ImageFormat.MimeType,                     
                                                filterContext.Image.Width, 
                                                filterContext.Image.Height, 
                                                filterContext.ClientHints.DPR);
            }
        }
    }
}
