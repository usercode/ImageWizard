using ImageWizard.Core.ImageFilters.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public interface IFilter
    {
        public FilterContext Context { get; set; }
    }
}
