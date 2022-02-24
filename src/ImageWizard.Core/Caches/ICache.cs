// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
    /// <summary>
    /// ICache
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// ReadAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ICachedData?> ReadAsync(string key);

        /// <summary>
        /// WriteAsync
        /// </summary>
        /// <param name="key"></param>
        /// <param name="metadata"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task WriteAsync(string key, IMetadata metadata, Stream stream);
    }
}
