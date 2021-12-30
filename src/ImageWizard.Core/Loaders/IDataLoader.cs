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
    /// IDataLoader
    /// </summary>
    public interface IDataLoader
    {
        /// <summary>
        /// RefreshSettings
        /// </summary>
        DataLoaderRefreshMode RefreshMode { get; }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedImage = null);
    }
}
