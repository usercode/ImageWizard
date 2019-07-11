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
            ResponseCacheControlMaxAge = TimeSpan.FromDays(365);
            Key = "DEMO-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---==";

            ImageMaxWidth = 2500;
            ImageMaxHeight = 2500;
        }

        /// <summary>
        /// Segment
        /// </summary>
        public PathString BasePath { get; set; }

        /// <summary>
        /// ResponseCacheTime
        /// </summary>
        public TimeSpan? ResponseCacheControlMaxAge { get; set; }

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
        /// ImageMaxWidth
        /// </summary>
        public int? ImageMaxWidth { get; set; }

        /// <summary>
        /// ImageMaxHeight
        /// </summary>
        public int? ImageMaxHeight { get; set; }

    }
}
