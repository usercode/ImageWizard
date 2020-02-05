using ImageWizard.Core.ImageLoaders;
using System;

namespace ImageWizard.Azure
{
    public class AzureBlobOptions : ImageLoaderOptions
    {
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }
}
