using ImageWizard.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Services.Types
{
    /// <summary>
    /// ImageMetadata
    /// </summary>
    public class ImageMetadata : IImageMetadata
    {
        public ImageMetadata()
        {
            NoImageCache = false;
            Cache = new CacheSettings();
        }

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Signature
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Hash
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// MimeType
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// FileLength
        /// </summary>
        public int FileLength { get; set; }

        /// <summary>
        /// DPR
        /// </summary>
        public double? DPR { get; set; }

        /// <summary>
        /// NoImageCache
        /// </summary>
        public bool NoImageCache { get; set; }
        /// <summary>
        /// CacheSettings
        /// </summary>
        public CacheSettings Cache { get; set; }

        /// <summary>
        /// Filters
        /// </summary>
        public string[] Filters { get; set; }

        /// <summary>
        /// DeliveryType
        /// </summary>
        public string LoaderType { get; set; }

        /// <summary>
        /// ImageSource
        /// </summary>
        public string LoaderSource { get; set; }
    }
}
