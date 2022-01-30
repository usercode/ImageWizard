using Amazon.S3;
using Amazon.S3.Model;
using ImageWizard.Core;
using ImageWizard.Loaders;
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
    public class AwsLoader : DataLoader<AwsOptions>
    {
        public AwsLoader(IOptions<AwsOptions> options)
            : base(options)
        {
            Client = new AmazonS3Client(Options.Value.AccessKeyId, Options.Value.SecretAccessKey);
        }

        /// <summary>
        /// Client
        /// </summary>
        public IAmazonS3 Client { get; }

        public override async Task<OriginalData> GetAsync(string source, ICachedData existingCachedImage)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = Options.Value.BucketName,
                Key = source
            };

            if (existingCachedImage != null)
            {
                request.EtagToNotMatch = $"\"{existingCachedImage.Metadata.Cache.ETag}\"";
            }

            GetObjectResponse result = await Client.GetObjectAsync(request);

            if (result.HttpStatusCode == System.Net.HttpStatusCode.NotModified)
            {
                result.Dispose();

                return null;
            }

            return new OriginalData(
                        result.Headers.ContentType,
                        result.ResponseStream,
                        new CacheSettings() { ETag = result.ETag.GetTagUnquoted() });
        }
    }
}
