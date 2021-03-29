using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageLoaders
{
    /// <summary>
    /// IImageLoader
    /// </summary>
    public interface IImageLoader
    {
        /// <summary>
        /// RefreshSettings
        /// </summary>
        ImageLoaderRefreshMode RefreshMode { get; }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        Task<OriginalImage?> GetAsync(string source, ICachedImage? existingCachedImage = null);
    }
}
