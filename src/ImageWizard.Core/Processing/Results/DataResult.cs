using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Processing.Results
{
    /// <summary>
    /// DataResult
    /// </summary>
    public class DataResult : IDisposable
    {
        public DataResult(Stream data, string mimeType)
        {
            MimeType = mimeType;
            Data = data;
        }

        /// <summary>
        /// MimeType
        /// </summary>
        public string MimeType { get; }

        /// <summary>
        /// Data
        /// </summary>
        public Stream Data { get; }

        public virtual void Dispose()
        {
            Data.Dispose();
        }
    }
}
