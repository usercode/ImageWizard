using ImageWizard.ImageFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public class FilterContext
    {
        public FilterContext(Image<Rgba32> image)
        {
            Image = image;
        }

        /// <summary>
        /// Image
        /// </summary>
        public Image<Rgba32> Image { get; }

        /// <summary>
        /// ImageFormat
        /// </summary>
        public IImageFormat ImageFormat { get; set; }
    }
}
