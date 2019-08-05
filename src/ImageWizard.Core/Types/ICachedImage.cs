using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.Types
{
    /// <summary>
    /// ICachedImage
    /// </summary>
    public interface ICachedImage
    {
        /// <summary>
        /// Metadata
        /// </summary>
        /// <returns></returns>
        IImageMetadata Metadata { get; }

        /// <summary>
        /// OpenReadAsync
        /// </summary>
        /// <returns></returns>
        Task<Stream> OpenReadAsync();
    }
}
