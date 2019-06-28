using ImageWizard.ImageFormats.Base;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageStorages
{
    /// <summary>
    /// IImageCache
    /// </summary>
    public interface IImageCache
    {
        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<CachedImage> GetAsync(string key);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="key"></param>
        /// <param name="originalImage"></param>
        /// <param name="imageFormat"></param>
        /// <param name="transformedImageData"></param>
        /// <returns></returns>
        Task<CachedImage> SaveAsync(string key, OriginalImage originalImage, IImageFormat imageFormat, byte[] transformedImageData);
    }
}
