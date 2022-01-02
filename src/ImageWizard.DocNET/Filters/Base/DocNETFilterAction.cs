using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.DocNET.Filters.Base
{
    public class DocNETFilterAction<TFilter> : FilterAction<TFilter>
         where TFilter : DocNETFilter
    {
        public DocNETFilterAction(IServiceProvider serviceProvider, Regex regex, MethodInfo method)
            : base(serviceProvider, regex, method)
        {
        }
    }
}
