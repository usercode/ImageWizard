using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public abstract class FilterBase<TContext> : IFilter
        where TContext : FilterContext
    {
        FilterContext? IFilter.Context 
        { 
            get => Context; 
            set => Context = (TContext?)value; 
        }

        public abstract string Namespace { get; }

        public TContext? Context { get; set; }
    }
}
