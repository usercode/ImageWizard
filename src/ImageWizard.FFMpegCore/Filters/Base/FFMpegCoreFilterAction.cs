using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageWizard.FFMpegCore.Filters.Base
{
    public class FFMpegCoreFilterAction<TFilter> : FilterAction<TFilter>
        where TFilter : FFMpegCoreFilter
    {
        public FFMpegCoreFilterAction(IServiceProvider serviceProvider, Regex regex, MethodInfo method)
            : base(serviceProvider, regex, method)
        {

        }
}
}
