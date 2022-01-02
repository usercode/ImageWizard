using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
{
    /// <summary>
    /// DataLoaderOptions
    /// </summary>
    public class DataLoaderOptions
    {
        public DataLoaderOptions()
        {
            RefreshMode = DataLoaderRefreshMode.None;
        }

        /// <summary>
        /// RefreshMode
        /// </summary>
        public DataLoaderRefreshMode RefreshMode { get; set; }

        /// <summary>
        /// CacheControl
        /// </summary>
        public TimeSpan? CacheControlMaxAge { get; set; }
    }
}
