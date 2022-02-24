// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Loaders
{
    /// <summary>
    /// ILoader
    /// </summary>
    public interface ILoader
    {
        IOptions<LoaderOptions> Options { get; }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedData = null);
    }
}
