using ImageWizard.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Services.Types
{
    /// <summary>
    /// OriginalImage
    /// </summary>
    public class OriginalImage
    {
        public OriginalImage(string url, string mimeType, byte[] data)
            : this(url, mimeType, data, new CacheSettings())
        {
        }

        public OriginalImage(string url, string mimeType, byte[] data, CacheSettings cacheSettings)
        {
            Url = url;
            MimeType = mimeType;
            Data = data;

            CacheSettings = cacheSettings;
        }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// MimeType
        /// </summary>
        public string MimeType { get; }

        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// ETag
        /// </summary>
        public CacheSettings CacheSettings { get; set; }
    }
}
