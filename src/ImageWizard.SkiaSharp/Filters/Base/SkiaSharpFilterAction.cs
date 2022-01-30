﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageWizard.SkiaSharp.Filters.Base
{
    public class SkiaSharpFilterAction<TFilter> : FilterAction<TFilter>
        where TFilter : SkiaSharpFilter
    {
        public SkiaSharpFilterAction(IServiceProvider serviceProvider, Regex regex, MethodInfo method)
            : base(serviceProvider, regex, method)
        {
        }
    }
}
