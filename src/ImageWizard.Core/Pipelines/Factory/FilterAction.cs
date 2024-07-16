// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ImageWizard.Processing;

public class FilterAction<TFilter> : IFilterAction
    where TFilter : IFilter
{
    public delegate void FilterItemHandler(TFilter filer, GroupCollection groups);

    public FilterAction(string name, [StringSyntax(StringSyntaxAttribute.Regex)]string pattern, FilterItemHandler handler)
    {
        Name = name;
        Regex = new Regex(pattern);
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
    public FilterItemHandler Handler { get; }

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
