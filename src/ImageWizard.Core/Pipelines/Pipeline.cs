﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing.Results;
using ImageWizard.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ImageWizard.Processing;

/// <summary>
/// Processing pipeline
/// </summary>
public abstract class Pipeline<TFilterBase, TFilterContext> : IPipeline
    where TFilterBase : Filter<TFilterContext>
    where TFilterContext : FilterContext
{
    public delegate void PreProcessing(TFilterContext context);
    public delegate void PostProcessing(TFilterContext context);

    public Pipeline(IServiceProvider serviceProvider, ILogger<Pipeline<TFilterBase, TFilterContext>> logger)
    {
        ServiceProvider = serviceProvider;
        Logger = logger;
    }

    /// <summary>
    /// ServiceProvider
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger<Pipeline<TFilterBase, TFilterContext>> Logger { get; }

    /// <summary>
    /// FilterFactories
    /// </summary>
    protected IDictionary<string, IList<IFilterAction>> FilterFactories { get; } = new Dictionary<string, IList<IFilterAction>>();

    public void AddFilter<TFilterFactory>()
        where TFilterFactory : IFilterFactory
    {
        IEnumerable<IFilterAction> items = TFilterFactory.Create();

        foreach (IFilterAction item in items)
        {
            if (FilterFactories.TryGetValue(item.Name, out IList<IFilterAction>? filterItems) == false)
            {
                filterItems = new List<IFilterAction>();

                FilterFactories.Add(item.Name, filterItems);
            }

            filterItems.Add(item);
        }
    }

    ///// <summary>
    ///// Adds filter to pipeline.
    ///// </summary>
    //public void AddFilter<TFilter>()
    //    where TFilter : TFilterBase
    //{
    //    MethodInfo[] methods = typeof(TFilter)
    //                                .GetMethods()
    //                                .Where(x => x.IsPublic)
    //                                .Where(x => x.GetCustomAttribute<FilterAttribute>() != null)
    //                                .ToArray();

    //    foreach (MethodInfo method in methods)
    //    {
    //        ParameterInfo[] parameters = method.GetParameters();

    //        Type[] integerTypes = [typeof(byte), typeof(short), typeof(int), typeof(long)];
    //        Type[] floatingNumberTypes = [typeof(float), typeof(double), typeof(decimal)];

    //        static ParameterItem CreateParameter(ParameterInfo pi, string pattern)
    //        {
    //            return new ParameterItem() { Name = pi.Name, Pattern = $@"(?<{pi.Name}>{pattern})" };
    //        }

    //        List<ParameterItem> pp = new List<ParameterItem>();

    //        for (int i = 0; i < parameters.Length; i++)
    //        {
    //            ParameterInfo currentParameter = parameters[i];

    //            if (integerTypes.Any(x => x == currentParameter.ParameterType))
    //            {
    //                pp.Add(CreateParameter(currentParameter, @"-?\d+"));
    //            }
    //            else if (floatingNumberTypes.Any(x => x == currentParameter.ParameterType))
    //            {
    //                pp.Add(CreateParameter(currentParameter, @"-?\d+\.\d+"));
    //            }
    //            else if (currentParameter.ParameterType == typeof(bool))
    //            {
    //                pp.Add(CreateParameter(parameters[i], "True|False"));
    //            }
    //            else if (currentParameter.ParameterType == typeof(string))
    //            {
    //                //find string as Base64Url or surrounded with ''
    //                ParameterItem p = CreateParameter(parameters[i], @"(('[^']*')|([A-Za-z0-9-_\s]+))");

    //                pp.Add(p);
    //            }
    //            else if (currentParameter.ParameterType.IsEnum)
    //            {
    //                //@"[a-z]{1}[a-z0-9]*"

    //                string[] enumValues = Enum.GetNames(parameters[i].ParameterType)
    //                                          .Select(x => x.ToLower())
    //                                          .ToArray();

    //                pp.Add(CreateParameter(parameters[i], string.Join("|", enumValues)));
    //            }
    //            else
    //            {
    //                throw new Exception("parameter type is not supported: " + parameters[i].ParameterType.Name);
    //            }
    //        }

    //        StringBuilder builder = new StringBuilder("^");

    //        //function begin
    //        builder.Append($@"{method.Name.ToLowerInvariant()}\(");

    //        bool optionalParameterCall = parameters.All(x => (x.DefaultValue is DBNull) == false);

    //        if (optionalParameterCall)
    //        {
    //            //add optional parameters
    //            builder.Append($"(,|{string.Join("|", pp.Select(x => $"({x.Name}={x.Pattern})").ToArray())})*");
    //        }
    //        else
    //        {
    //            //add all parameters
    //            builder.Append(string.Join(",", pp.Select(x => x.Pattern).ToArray()));
    //        }

    //        //function end
    //        builder.Append(@"\)$");

    //        IFilterAction<TFilterContext> filterAction = CreateFilterAction<TFilter>(new Regex(builder.ToString(), RegexOptions.Compiled), method);

    //        FilterActions.Add(filterAction);
    //    }
    //}
    /// <summary>
    /// Creates filter context.
    /// </summary>
    protected abstract Task<TFilterContext> CreateFilterContext(PipelineContext context);

    /// <summary>
    /// Starts processing pipeline.
    /// </summary>
    public async Task<DataResult> StartAsync(PipelineContext context)
    {
        using TFilterContext filterContext = await CreateFilterContext(context);

        //execute preprocessing
        PreProcessing? preProcessing = ServiceProvider.GetService<PreProcessing>();
        preProcessing?.Invoke(filterContext);

        //execute filters
        while (context.UrlFilters.TryPeek(out FilterSegment segment))
        {
            if (FilterFactories.TryGetValue(segment.Name, out var filters))
            {
                //find and execute filter
                IFilterAction? foundFilter = filters.FirstOrDefault(x => x.TryExecute(ServiceProvider, segment.Parameter, filterContext));

                if (foundFilter != null)
                {
                    Logger.LogTrace("Filter executed: {filter}", segment);

                    filterContext.ProcessingContext.UrlFilters.Dequeue();
                }
                else
                {
                    Logger.LogTrace("filter overload was not found: {filter}", segment);

                    throw new Exception($"Filter overload was not found: {segment}");
                }
            }
            else
            {
                Logger.LogTrace("filter was not found: {filter}", segment);

                throw new Exception($"Filter was not found: {segment}");
            }

            //stop processing?
            if (filterContext.Result != null)
            {
                return filterContext.Result;
            }
        }

        //execute postprocessing
        PostProcessing? postProcessing = ServiceProvider.GetService<PostProcessing>();
        postProcessing?.Invoke(filterContext);

        return await filterContext.BuildResultAsync();
    }
}
