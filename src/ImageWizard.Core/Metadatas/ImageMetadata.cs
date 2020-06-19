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
    public class ImageMetadata : IMetadata
    {
        public ImageMetadata()
        {
            Cache = new CacheSettings();
        }

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime? Created { get; set; }

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
        /// Width
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// FileLength
        /// </summary>
        public int FileLength { get; set; }

        /// <summary>
        /// DPR
        /// </summary>
        public double? DPR { get; set; }

        /// <summary>
        /// CacheSettings
        /// </summary>
        public CacheSettings Cache { get; set; }

        /// <summary>
        /// Filters
        /// </summary>
        public IEnumerable<string> Filters { get; set; }

        /// <summary>
        /// LoaderType
        /// </summary>
        public string LoaderType { get; set; }

        /// <summary>
        /// LoaderSource
        /// </summary>
        public string LoaderSource { get; set; }
    }
}
