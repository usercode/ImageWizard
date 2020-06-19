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
        where TFilter : ImageSharpFilter, new()
    {
        public ImageSharpFilterAction(Regex regex, MethodInfo method)
            : base(regex, method)
        {
        }
    }
}
