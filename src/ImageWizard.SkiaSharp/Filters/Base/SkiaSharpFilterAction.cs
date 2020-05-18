using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageWizard.SkiaSharp.Filters.Base
{
    public class SkiaSharpFilterAction<TFilter> : FilterAction<TFilter>
        where TFilter : SkiaSharpFilter, new()
    {
        public SkiaSharpFilterAction(Regex regex, MethodInfo method)
            : base(regex, method)
        {
        }
    }
}
