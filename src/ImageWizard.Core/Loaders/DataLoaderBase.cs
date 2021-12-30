using ImageWizard.Core.Types;
using ImageWizard.ImageLoaders;
using ImageWizard.Services.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageLoaders
{
    public abstract class DataLoaderBase : IDataLoader
    {
        public DataLoaderBase()
        {
            RefreshMode = DataLoaderRefreshMode.None;
        }

        public virtual DataLoaderRefreshMode RefreshMode { get; }

        public abstract Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedImage);
    }
}
