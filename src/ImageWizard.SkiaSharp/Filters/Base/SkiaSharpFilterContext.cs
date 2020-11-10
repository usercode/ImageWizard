using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using ImageWizard.ImageFormats.Base;
using ImageWizard.Services.Types;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.SkiaSharp.Filters.Base
{
    /// <summary>
    /// SkiaSharpFilterContext
    /// </summary>
    public class SkiaSharpFilterContext : FilterContext
    {
        public SkiaSharpFilterContext(ProcessingPipelineContext processingContext, SKBitmap image, IImageFormat imageFormat)
            : base(processingContext)
        {
            Image = image;
            ImageFormat = imageFormat;
        }

        private SKBitmap _image;

        /// <summary>
        /// Image
        /// </summary>
        public SKBitmap Image 
        {
            get => _image;
            set
            {
                if(_image != null)
                {
                    _image.Dispose();
                }

                _image = value;
            }
        }

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
            MemoryStream mem = new MemoryStream();
            ImageFormat.SaveImage(Image, mem);

            return new ImageResult(
                                    mem.ToArray(),
                                    ImageFormat.MimeType,
                                    Image.Width,
                                    Image.Height,
                                    ProcessingContext.ClientHints.DPR);
        }
    }
}
