// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageWizard.Loaders;
using Microsoft.Extensions.Options;

namespace ImageWizard.Azure;

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

    public override async Task<LoaderResult> GetAsync(string source, ICachedData existingCachedImage)
    {
        BlobClient blob = Client.GetBlobClient(source);

        BlobRequestConditions conditions = new BlobRequestConditions();

        if (existingCachedImage != null)
        {
            conditions.IfNoneMatch = new ETag(existingCachedImage.Metadata.Cache.ETag);
        }

        var response = await blob.DownloadContentAsync(new BlobDownloadOptions() { Conditions = conditions });

        if (response.GetRawResponse().Status == (int)System.Net.HttpStatusCode.NotModified)
        {
            return LoaderResult.NotModified();
        }

        return LoaderResult.Success(new OriginalData(
                    response.Value.Details.ContentType,
                    response.Value.Content.ToStream(),
                    new CacheSettings() { ETag = response.Value.Details.ETag.ToString().GetTagUnquoted() }));
    }
}
