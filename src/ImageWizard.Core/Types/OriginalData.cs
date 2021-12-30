using ImageWizard.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Services.Types
{
    /// <summary>
    /// OriginalData
    /// </summary>
    public class OriginalData
    {
        public OriginalData(string mimeType, byte[] data)
            : this(mimeType, data, new CacheSettings())
        {
        }

        public OriginalData(string mimeType, byte[] data, CacheSettings cacheSettings)
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
