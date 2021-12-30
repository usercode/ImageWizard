using ImageWizard.Core.ImageLoaders;
using System;

namespace ImageWizard.Azure
{
    public class AzureBlobOptions : DataLoaderOptions
    {
        public AzureBlobOptions()
        {
            RefreshMode = DataLoaderRefreshMode.EveryTime;
        }

        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }
}
