using ImageWizard.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Types
{
    /// <summary>
    /// CachedImage
    /// </summary>
    public class CachedImage : ICachedImage
    {
        public CachedImage(IImageMetadata metadata, Func<Task<Stream>> streamTask)
        {
            Metadata = metadata;
            StreamTask = streamTask;
        }

        /// <summary>
        /// Metadata
        /// </summary>
        public IImageMetadata Metadata { get; }

        /// <summary>
        /// Buffer
        /// </summary>
        private Func<Task<Stream>> StreamTask { get; }

        public Task<Stream> OpenReadAsync()
        {
            return StreamTask();
        }
    }
}
