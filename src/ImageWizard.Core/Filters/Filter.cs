// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

public abstract class Filter<TContext> : IFilter
    where TContext : FilterContext
{
    public abstract string Namespace { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public TContext Context { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    FilterContext IFilter.Context { get => Context; set => Context = (TContext)value; }
}
