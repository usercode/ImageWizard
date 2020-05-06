using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Settings;
using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.Settings;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    /// <summary>
    /// FilterContext
    /// </summary>
    public class ImageFilterContext : FilterContext
    {
        public ImageFilterContext(Image image, IImageFormat imageFormat, ClientHints clientHints)
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

       
    }
}
