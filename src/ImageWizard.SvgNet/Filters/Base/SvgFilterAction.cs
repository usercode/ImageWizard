using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    /// <summary>
    /// FilterAction
    /// </summary>
    public class SvgFilterAction<TFilter> : FilterAction<TFilter>
      where TFilter : SvgFilter
    {
        public SvgFilterAction(IServiceProvider serviceProvider, Regex regex, MethodInfo method)
            : base(serviceProvider, regex, method)
        {
        }

    }
}
