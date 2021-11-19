using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.OpenCvSharp.Filters
{
    /// <summary>
    /// FilterAction
    /// </summary>
    public class OpenCvSharpFilterAction<TFilter> : FilterAction<TFilter>
        where TFilter : OpenCvSharpFilter
    {
        public OpenCvSharpFilterAction(IServiceProvider serviceProvider, Regex regex, MethodInfo method)
            : base(serviceProvider, regex, method)
        {
        }
    }
}
