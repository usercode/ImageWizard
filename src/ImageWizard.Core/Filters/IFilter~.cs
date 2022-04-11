// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard;

public interface IFilter<TContext> : IFilter
    where TContext : FilterContext
{
    public TContext Context { get; internal set; }
}
