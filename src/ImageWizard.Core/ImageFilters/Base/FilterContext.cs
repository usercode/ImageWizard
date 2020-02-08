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

namespace ImageWizard.Filters
{
    /// <summary>
    /// FilterContext
    /// </summary>
    public class FilterContext
    {
        public FilterContext(ImageWizardOptions settings, Image image, IImageFormat imageFormat, ClientHints clientHints)
        {
            Settings = settings;
            Image = image;
            ImageFormat = imageFormat;
            NoImageCache = false;
            ClientHints = clientHints;
        }

        /// <summary>
        /// Settings
        /// </summary>
        public ImageWizardOptions Settings { get; }

        /// <summary>
        /// Image
        /// </summary>
        public Image Image { get; }

        /// <summary>
        /// ImageFormat
        /// </summary>
        public IImageFormat ImageFormat { get; set; }        

        /// <summary>
        /// NoCache
        /// </summary>
        public bool NoImageCache { get; set; }

       

        /// <summary>
        /// ClientHints
        /// </summary>
        public ClientHints ClientHints { get; set; }
    }
}
