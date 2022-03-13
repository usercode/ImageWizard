// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    /// <summary>
    /// FilterContext
    /// </summary>
    public class ImageSharpFilterContext : FilterContext
    {
        public ImageSharpFilterContext(
                    PipelineContext processingContext, 
                    Image image, 
                    IImageFormat imageFormat,
                    IOptions<ImageSharpOptions> options)
                    : base(processingContext)
        {
            Image = image;
            ImageFormat = imageFormat;
            Options = options;
        }

        /// <summary>
        /// Image
        /// </summary>
        public Image Image { get; }

        /// <summary>
        /// ImageFormat
        /// </summary>
        public IImageFormat ImageFormat { get; set; }

        /// <summary>
        /// Options
        /// </summary>
        public IOptions<ImageSharpOptions> Options { get; }

        public override void Dispose()
        {
            Image.Dispose();
        }

        public override async Task<DataResult> BuildResultAsync()
        {
            //check max width and height
            bool change = false;

            int width = Image.Width;
            int height = Image.Height;

            if (Options.Value.ImageMaxWidth != null && width > Options.Value.ImageMaxWidth)
            {
                change = true;
                width = Options.Value.ImageMaxWidth.Value;
            }

            if (Options.Value.ImageMaxHeight != null && height > Options.Value.ImageMaxHeight)
            {
                change = true;
                height = Options.Value.ImageMaxHeight.Value;
            }

            if (change == true)
            {
                Image.Mutate(x => x.Resize(new ResizeOptions() { Mode = ResizeMode.Max, Size = new Size(width, height) }));
            }           

            Stream mem = ProcessingContext.StreamPool.GetStream();

            //save image
            await ImageFormat.SaveImageAsync(Image, mem);

            mem.Seek(0, SeekOrigin.Begin);

            //update some metadata
            //ProcessingContext.DisableCache = NoImageCache;
            return new ImageResult(
                                        mem,
                                        ImageFormat.MimeType,
                                        Image.Width,
                                        Image.Height);
        }
    }
}
