// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

public interface IFilter<TContext> : IFilter
    where TContext : FilterContext
{
    public TContext Context { get; internal set; }
}
