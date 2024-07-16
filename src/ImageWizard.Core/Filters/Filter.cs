// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

public abstract class Filter<TContext> : IFilter
    where TContext : FilterContext
{
    public abstract string Namespace { get; }

    public TContext Context { get; set; }

    FilterContext IFilter.Context { get => Context; set => Context = (TContext)value; }
}
