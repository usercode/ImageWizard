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
            CacheSettings = new CacheSettings();
        }

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Signature
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// MimeType
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// ImageSource
        /// </summary>
        public string ImageSource { get; set; }

        /// <summary>
        /// DPR
        /// </summary>
        public double? DPR { get; set; }

        /// <summary>
        /// CacheSettings
        /// </summary>
        public CacheSettings CacheSettings { get; set; }
    }
}
