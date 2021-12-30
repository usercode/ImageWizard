using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders
{
    /// <summary>
    /// Refresh strategy for the original image.
    /// </summary>
    public enum DataLoaderRefreshMode
    {
        /// <summary>
        /// The cached image will be never refreshed. (Recommend for fingerprint strategy.)
        /// </summary>
        None = 0,

        /// <summary>
        /// Checks every time for a new version of the original image.
        /// </summary>
        EveryTime = 1,

        /// <summary>
        /// Use cache control information of the original image source.
        /// </summary>
        UseRemoteCacheControl = 2,
    }
}
