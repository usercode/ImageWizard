using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageFilters.Base
{
    /// <summary>
    /// IFilterAction
    /// </summary>
    public interface IFilterAction
    {
        /// <summary>
        /// TryExecute
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        bool TryExecute(string input, FilterContext filterContext);
    }
}
