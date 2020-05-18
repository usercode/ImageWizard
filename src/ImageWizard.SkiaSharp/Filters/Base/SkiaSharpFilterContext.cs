using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Settings;
using ImageWizard.ImageFormats.Base;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp.Filters.Base
{
    /// <summary>
    /// SkiaSharpFilterContext
    /// </summary>
    public class SkiaSharpFilterContext : FilterContext
    {
        public SkiaSharpFilterContext(SKBitmap image, IImageFormat imageFormat, ClientHints clientHints)
        {
            Image = image;
            ImageFormat = imageFormat;
            ClientHints = clientHints;
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

        public IImageFormat ImageFormat { get; set; }
    }
}
