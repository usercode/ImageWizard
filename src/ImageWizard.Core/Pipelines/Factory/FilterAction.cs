// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace ImageWizard.Processing;

public class FilterAction<TFilter> : IFilterAction
    where TFilter : IFilter
{
    public delegate void FilterActionHandler(TFilter filer, GroupCollection groups);

    public FilterAction(string name, Regex pattern, FilterActionHandler handler)
    {
        Name = name;
        Regex = pattern;
        Handler = handler;
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Regex
    /// </summary>
    public Regex Regex { get; }

    /// <summary>
    /// Handler
    /// </summary>
    public FilterActionHandler Handler { get; }

    public bool TryExecute(IServiceProvider serviceProvider, string input, FilterContext filterContext)
    {
        Match match = Regex.Match(input);

        if (match.Success == false)
        {
            return false;
        }

        TFilter filter = serviceProvider.GetRequiredService<TFilter>();
        filter.Context = filterContext;

        Handler(filter, match.Groups);

        return true;
    }
}
