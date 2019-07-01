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
            ResponseCacheTime = TimeSpan.FromDays(90);
            Key = "DEMO-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---==";
        }

        /// <summary>
        /// Segment
        /// </summary>
        public PathString BasePath { get; set; }

        /// <summary>
        /// ResponseCacheTime
        /// </summary>
        public TimeSpan? ResponseCacheTime { get; set; }

        /// <summary>
        /// AllowUnsafeUrl
        /// </summary>
        public bool AllowUnsafeUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool UseETag { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

    }
}
