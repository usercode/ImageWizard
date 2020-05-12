using ImageWizard.Core.ImageLoaders;
using System;

namespace ImageWizard.AWS
{
    /// <summary>
    /// AwsOptions
    /// </summary>
    public class AwsOptions : ImageLoaderOptions
    {
        public AwsOptions()
        {
            RefreshMode = ImageLoaderRefreshMode.EveryTime;
        }

        /// <summary>
        /// AccessKeyId
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// SecretAccessKey
        /// </summary>
        public string SecretAccessKey { get; set; }

        /// <summary>
        /// BucketName
        /// </summary>
        public string BucketName { get; set; }
    }
}
