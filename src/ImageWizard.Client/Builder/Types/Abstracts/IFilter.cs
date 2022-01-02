using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client
{
    /// <summary>
    /// IFilter
    /// </summary>
    public interface IFilter : IBuildUrl, IUrlBuilder
    {
        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IFilter Filter(string filter);
    }
}
