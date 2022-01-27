using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
    /// <summary>
    /// ICachedDataHash
    /// </summary>
    public interface ICachedDataHash
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> CreateAsync(Stream stream);
    }
}
