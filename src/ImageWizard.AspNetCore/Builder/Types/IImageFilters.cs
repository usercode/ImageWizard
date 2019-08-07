using ImageWizard.SharedContract.FilterTypes;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore.Builder.Types
{
    /// <summary>
    /// IImageFilters
    /// </summary>
    public interface IImageFilters : IImageBuildUrl, IImageUrlBuilder
    {
        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IImageFilters Filter(string filter);
    }
}
