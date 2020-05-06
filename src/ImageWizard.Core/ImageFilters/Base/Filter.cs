using ImageWizard.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageFilters.Base
{
    public abstract class Filter<TContext> : IFilter
        where TContext : FilterContext
    {
        FilterContext IFilter.Context 
        { 
            get => Context; 
            set => Context = (TContext)value; 
        }

        public TContext Context { get; set; }
    }
}
