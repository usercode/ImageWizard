﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard
{
    public interface IFilter
    {
        public FilterContext? Context { get; set; }
    }
}
