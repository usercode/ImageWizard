using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.Settings
{
    public class CacheControl
    {
        public CacheControl()
        {
            IsEnabled = true;
            Public = true;
            MaxAge = TimeSpan.FromDays(365);
            MustRevalidate = false;
        }

        /// <summary>
        /// IsEnabled
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Public
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// ResponseCacheTime
        /// </summary>
        public TimeSpan? MaxAge { get; set; }

        /// <summary>
        /// CacheControlMustRevalidate
        /// </summary>
        public bool MustRevalidate { get; set; }
    }
}
