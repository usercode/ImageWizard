﻿using ImageWizard.Metadatas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.Types
{
    /// <summary>
    /// ICachedData
    /// </summary>
    public interface ICachedData
    {
        /// <summary>
        /// Metadata
        /// </summary>
        /// <returns></returns>
        IMetadata Metadata { get; }

        /// <summary>
        /// OpenReadAsync
        /// </summary>
        /// <returns></returns>
        Task<Stream> OpenReadAsync();
    }
}
