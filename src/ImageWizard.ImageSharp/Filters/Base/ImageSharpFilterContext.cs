using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.Services.Types;
using ImageWizard.Settings;
using ImageWizard.Utils.FilterTypes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
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
        public ImageSharpFilterContext(ProcessingPipelineContext processingContext, Image image, IImageFormat imageFormat, ClientHints clientHints)
            : base(processingContext)
        {
            Image = image;
            ImageFormat = imageFormat;
            NoImageCache = false;
            ClientHints = clientHints;
        }

        /// <summary>
        /// Image
        /// </summary>
        public Image Image { get; }

        /// <summary>
        /// ImageFormat
        /// </summary>
        public IImageFormat ImageFormat { get; set; }

        public override void Dispose()
        {
            if(Image != null)
            {
                Image.Dispose();
            }
        }

        public override async Task<ImageResult> BuildResultAsync()
        {
            //check max width and height
            bool change = false;

            int width = Image.Width;
            int height = Image.Height;

            //if (ProcessingContext.ImageWizardOptions.ImageMaxWidth != null && width > Options.ImageMaxWidth)
            //{
            //    change = true;
            //    width = Options.ImageMaxWidth.Value;
            //}

            //if (Options.ImageMaxHeight != null && height > Options.ImageMaxHeight)
            //{
            //    change = true;
            //    height = Options.ImageMaxHeight.Value;
            //}

            //if (change == true)
            //{
            //    new ResizeFilter() { Context = filterContext }.Resize(width, height, ResizeMode.Max);
            //}

            MemoryStream mem = new MemoryStream();

            //save image
            await ImageFormat.SaveImageAsync(Image, mem);

            byte[] transformedImageData = mem.ToArray();

            //update some metadata
            //ProcessingContext.DisableCache = NoImageCache;
            return new ImageResult(
                                            transformedImageData,
                                            ImageFormat.MimeType,
                                            Image.Width,
                                            Image.Height,
                                            ClientHints.DPR);
        }
    }
}
