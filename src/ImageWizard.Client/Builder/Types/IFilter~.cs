using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client.Builder.Types
{
    /// <summary>
    /// IFilter
    /// </summary>
    public interface IFilter<T> : IFilter
        where T : IFilter
    {
        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        new T Filter(string filter);
    }
}
