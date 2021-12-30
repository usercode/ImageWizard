using Amazon.S3;
using Amazon.S3.Model;
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

namespace ImageWizard.AWS
{
    /// <summary>
    /// AwsLoader
    /// </summary>
    public class AwsLoader : DataLoaderBase
    {
        public AwsLoader(IOptions<AwsOptions> options)
        {
            Options = options.Value;

            Client = new AmazonS3Client(Options.AccessKeyId, Options.SecretAccessKey);
        }

        /// <summary>
        /// Options
        /// </summary>
        public AwsOptions Options { get; }

        /// <summary>
        /// Client
        /// </summary>
        public IAmazonS3 Client { get; }

        public override async Task<OriginalData> GetAsync(string source, ICachedData existingCachedImage)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = Options.BucketName,
                Key = source
            };

            if (existingCachedImage != null)
            {
                request.EtagToNotMatch = $"\"{existingCachedImage.Metadata.Cache.ETag}\"";
            }

            GetObjectResponse result = await Client.GetObjectAsync(request);

            if(result.HttpStatusCode == System.Net.HttpStatusCode.NotModified)
            {
                return null;
            }

            byte[] data = await result.ResponseStream.ToByteArrayAsync();

            return new OriginalData(
                        result.Headers.ContentType,
                        await result.ResponseStream.ToByteArrayAsync(),
                        new CacheSettings() { ETag = result.ETag.GetTagUnquoted() });
        }
    }
}
