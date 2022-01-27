using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
    /// <summary>
    /// ICachedDataKey
    /// </summary>
    public interface ICachedDataKey
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Create(string input);
    }
}
