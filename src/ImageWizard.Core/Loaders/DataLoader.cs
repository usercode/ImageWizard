using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Loaders
{
    /// <summary>
    /// DataLoader
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public abstract class DataLoader<TOptions> : IDataLoader
        where TOptions : DataLoaderOptions
    {
        public DataLoader(IOptions<TOptions> options)
        {
            Options = options;
        }

        /// <summary>
        /// Options
        /// </summary>
        public IOptions<TOptions> Options { get; }

        IOptions<DataLoaderOptions> IDataLoader.Options => Options;

        public abstract Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedData);
    }
}
