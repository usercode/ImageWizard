using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Loaders
{
    /// <summary>
    /// IDataLoader
    /// </summary>
    public interface IDataLoader
    {
        IOptions<DataLoaderOptions> Options { get; }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedData = null);
    }
}
