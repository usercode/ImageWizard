using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Loaders
{
    public abstract class DataLoaderBase<TOptions> : IDataLoader
        where TOptions : DataLoaderOptions
    {
        public DataLoaderBase(IOptions<TOptions> options)
        {
            Options = options;
        }

        public IOptions<TOptions> Options { get; }

        IOptions<DataLoaderOptions> IDataLoader.Options => Options;

        public abstract Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedData);
    }
}
