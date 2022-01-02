using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// IFilterAction
    /// </summary>
    public interface IFilterAction<TContext>
    {
        /// <summary>
        /// TryExecute
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        bool TryExecute(string input, TContext filterContext);
    }
}
