using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public abstract class Filter<TContext> : IFilter<TContext>
        where TContext : FilterContext
    {
        public abstract string Namespace { get; }

        public TContext? Context { get; set; }
    }
}
