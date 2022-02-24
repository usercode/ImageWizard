// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageWizard;
using ImageWizard.Loaders;
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
    public class AzureBlobLoader : Loader<AzureBlobOptions>
    {
        public AzureBlobLoader(IOptions<AzureBlobOptions> options)
            : base(options)
        {
            Client = new BlobContainerClient(options.Value.ConnectionString, options.Value.ContainerName);
        }

        private BlobContainerClient Client { get; }

        public override async Task<OriginalData> GetAsync(string source, ICachedData existingCachedImage)
        {
            BlobClient blob = Client.GetBlobClient(source);

            BlobRequestConditions conditions = new BlobRequestConditions();

            if (existingCachedImage != null)
            {
                conditions.IfNoneMatch = new ETag(existingCachedImage.Metadata.Cache.ETag);
            }

            var response = await blob.DownloadStreamingAsync(conditions: conditions);

            if (response.GetRawResponse().Status == (int)System.Net.HttpStatusCode.NotModified)
            {
                response.Value.Dispose();

                return null;
            }

            return new OriginalData(
                        response.Value.Details.ContentType,
                        response.Value.Content,
                        new CacheSettings() { ETag = response.Value.Details.ETag.ToString().GetTagUnquoted() });
        }
    }
}
