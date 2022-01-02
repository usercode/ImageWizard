using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard
{
    public interface IFilter<TContext> : IFilter
    {
        public new TContext Context { get; set; }
    }
}
