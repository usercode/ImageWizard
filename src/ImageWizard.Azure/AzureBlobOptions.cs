using ImageWizard.Core.ImageLoaders;
using System;

namespace ImageWizard.Azure
{
    public class AzureBlobOptions : ImageLoaderOptions
    {
        public AzureBlobOptions()
        {
            RefreshMode = ImageLoaderRefreshMode.EveryTime;
        }

        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }
}
