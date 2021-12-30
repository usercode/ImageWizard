using ImageWizard.Core.Types;
using ImageWizard.Metadatas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Types
{
    public delegate Task<Stream> OpenStreamHandler();

    /// <summary>
    /// CachedData
    /// </summary>
    public class CachedData : ICachedData
    {
        public CachedData(IMetadata metadata, OpenStreamHandler streamTask)
        {
            Metadata = metadata;
            StreamTask = streamTask;
        }

        /// <summary>
        /// Metadata
        /// </summary>
        public IMetadata Metadata { get; }

        /// <summary>
        /// Buffer
        /// </summary>
        private OpenStreamHandler StreamTask { get; }

        /// <summary>
        /// Opens readonly stream
        /// </summary>
        /// <returns></returns>
        public Task<Stream> OpenReadAsync()
        {
            return StreamTask();
        }
    }
}
