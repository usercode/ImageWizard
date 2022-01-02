using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
    /// <summary>
    /// ICache
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// ReadAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ICachedData?> ReadAsync(string key);

        /// <summary>
        /// WriteAsync
        /// </summary>
        /// <param name="key"></param>
        /// <param name="originalImage"></param>
        /// <param name="imageFormat"></param>
        /// <param name="transformedImageData"></param>
        /// <returns></returns>
        Task WriteAsync(string key, ICachedData cachedData);
    }
}
