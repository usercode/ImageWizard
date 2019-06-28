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
    public class ImageWizardCoreSettings
    {
        public ImageWizardCoreSettings()
        {
            BasePath = "/image";
            AllowUnsafeUrl = false;
            ResponseCacheTime = TimeSpan.FromDays(7);
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
        /// Key
        /// </summary>
        public string Key { get; set; }

    }
}
