using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageWizard;
using ImageWizard.Core;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Azure
{
    /// <summary>
    /// AzureBlobLoader
    /// </summary>
    public class AzureBlobLoader : ImageLoaderBase
    {
        public AzureBlobLoader(IOptions<AzureBlobOptions> options)
        {
            Options = options.Value;

            Client = new BlobContainerClient(options.Value.ConnectionString, options.Value.ContainerName);
        }

        private AzureBlobOptions Options { get; }

        private BlobContainerClient Client { get; }

        public override ImageLoaderRefreshMode RefreshMode => Options.RefreshMode;

        public override async Task<OriginalImage> GetAsync(string source, ICachedImage existingCachedImage)
        {
            BlobClient blob = Client.GetBlobClient(source);

            BlobRequestConditions conditions = new BlobRequestConditions();

            if (existingCachedImage != null)
            {
                conditions.IfNoneMatch = new ETag(existingCachedImage.Metadata.Cache.ETag);
            }

            var response = await blob.DownloadAsync(conditions: conditions);

            if (response.GetRawResponse().Status == (int)System.Net.HttpStatusCode.NotModified)
            {
                return null;
            }

            return new OriginalImage(
                        response.Value.ContentType, 
                        await response.Value.Content.ToByteArrayAsync(), 
                        new CacheSettings() { ETag = response.Value.Details.ETag.ToString().GetTagUnquoted() });
        }
    }
}
