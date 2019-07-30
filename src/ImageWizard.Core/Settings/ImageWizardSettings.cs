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
    /// ServiceSettings
    /// </summary>
    public class ImageWizardSettings
    {
        public ImageWizardSettings()
        {
            BasePath = "/image";
            UseETag = true;
            AllowUnsafeUrl = false;
           
            Key = "DEMO-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---==";

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
        /// Segment
        /// </summary>
        public PathString BasePath { get; set; }

        /// <summary>
        /// AllowUnsafeUrl
        /// </summary>
        public bool AllowUnsafeUrl { get; set; }

        /// <summary>
        /// UseETag
        /// </summary>
        public bool UseETag { get; set; }

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
