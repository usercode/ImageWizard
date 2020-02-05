using ImageWizard.Core.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Settings
{
    /// <summary>
    /// ImageWizardOptions
    /// </summary>
    public class ImageWizardOptions
    {
        public ImageWizardOptions()
        {
            UseETag = true;
            UseClintHints = true;
            AllowUnsafeUrl = false;
            Key = null;

            ImageMaxWidth = 4000;
            ImageMaxHeight = 4000;

            AllowedDPR = new[] { 1.0, 1.5, 2.0, 3.0, 4.0 };

            CacheControl = new CacheControl();
        }

        /// <summary>
        /// CacheControl
        /// </summary>
        public CacheControl CacheControl { get; }

        /// <summary>
        /// AllowUnsafeUrl
        /// </summary>
        public bool AllowUnsafeUrl { get; set; }

        /// <summary>
        /// UseETag
        /// </summary>
        public bool UseETag { get; set; }

        /// <summary>
        /// UseClintHints
        /// </summary>
        public bool UseClintHints { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// AllowedDPR
        /// </summary>
        public double[] AllowedDPR { get; set; }

        /// <summary>
        /// ImageMaxWidth
        /// </summary>
        public int? ImageMaxWidth { get; set; }

        /// <summary>
        /// ImageMaxHeight
        /// </summary>
        public int? ImageMaxHeight { get; set; }

    }
}
