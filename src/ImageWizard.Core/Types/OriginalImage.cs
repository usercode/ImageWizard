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
        public OriginalImage(string mimeType, byte[] data)
            : this(mimeType, data, new CacheSettings())
        {
        }

        public OriginalImage(string mimeType, byte[] data, CacheSettings cacheSettings)
        {
            MimeType = mimeType;
            Data = data;

            Cache = cacheSettings ?? throw new ArgumentNullException(nameof(cacheSettings));
        }

        /// <summary>
        /// MimeType
        /// </summary>
        public string MimeType { get; }

        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// Cache
        /// </summary>
        public CacheSettings Cache { get; }
    }
}
