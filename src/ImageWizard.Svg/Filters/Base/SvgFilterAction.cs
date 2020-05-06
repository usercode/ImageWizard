using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageFilters.Base.Attributes;
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
      where TFilter : SvgFilter, new()
    {
        public SvgFilterAction(Regex regex, MethodInfo method)
            : base(regex, method)
        {
        }

    }
}
