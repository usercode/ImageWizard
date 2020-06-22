using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    /// <summary>
    /// FilterAction
    /// </summary>
    public class ImageSharpFilterAction<TFilter> : FilterAction<TFilter>
        where TFilter : ImageSharpFilter
    {
        public ImageSharpFilterAction(IServiceProvider serviceProvider, Regex regex, MethodInfo method)
            : base(serviceProvider, regex, method)
        {
        }
    }
}
