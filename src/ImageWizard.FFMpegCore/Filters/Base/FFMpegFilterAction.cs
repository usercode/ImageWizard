using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageWizard.FFMpegCore.Filters.Base
{
    public class FFMpegFilterAction<TFilter> : FilterAction<TFilter>
        where TFilter : FFMpegFilter
    {
        public FFMpegFilterAction(IServiceProvider serviceProvider, Regex regex, MethodInfo method)
            : base(serviceProvider, regex, method)
        {

        }
}
}
