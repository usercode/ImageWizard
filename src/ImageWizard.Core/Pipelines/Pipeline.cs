// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.Helpers;
using ImageWizard.Processing.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageWizard.Processing;

/// <summary>
/// Processing pipeline
/// </summary>
public abstract class Pipeline<TFilterBase, TFilterContext> : IPipeline
    where TFilterBase : IFilter<TFilterContext>
    where TFilterContext : FilterContext
{
    public delegate void PreProcessing(TFilterContext context);
    public delegate void PostProcessing(TFilterContext context);

    public Pipeline(IServiceProvider serviceProvider, ILogger<Pipeline<TFilterBase, TFilterContext>> logger)
    {
        ServiceProvider = serviceProvider;
        Logger = logger;

        FilterActions = new List<IFilterAction<TFilterContext>>();
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
    /// FilterActions
    /// </summary>
    protected IList<IFilterAction<TFilterContext>> FilterActions { get; }

    /// <summary>
    /// Adds filter to pipeline.
    /// </summary>
    public void AddFilter<TFilter>()
        where TFilter : TFilterBase
    {
        MethodInfo[] methods = typeof(TFilter)
                                    .GetMethods()
                                    .Where(x => x.IsPublic)
                                    .Where(x=> x.GetCustomAttribute<FilterAttribute>() != null)
                                    .ToArray();

        foreach (MethodInfo method in methods)
        {
            ParameterInfo[] parameters = method.GetParameters();

            Type[] integerTypes = [typeof(byte), typeof(short), typeof(int), typeof(long)];
            Type[] floatingNumberTypes = [typeof(float), typeof(double), typeof(decimal)];

            static ParameterItem CreateParameter(ParameterInfo pi, string pattern)
            {
                return new ParameterItem() { Name = pi.Name, Pattern = $@"(?<{pi.Name}>{pattern})" };
            }

            List<ParameterItem> pp = new List<ParameterItem>();

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo currentParameter = parameters[i];

                if (integerTypes.Any(x => x == currentParameter.ParameterType))
                {
                    pp.Add(CreateParameter(currentParameter, @"-?\d+"));
                }
                else if (floatingNumberTypes.Any(x => x == currentParameter.ParameterType))
                {
                    pp.Add(CreateParameter(currentParameter, @"-?\d+\.\d+"));
                }
                else if (currentParameter.ParameterType == typeof(bool))
                {
                    pp.Add(CreateParameter(parameters[i], "True|False"));
                }
                else if (currentParameter.ParameterType == typeof(string))
                {
                    //find string as Base64Url or surrounded with ''
                    ParameterItem p = CreateParameter(parameters[i], @"(('[^']*')|([A-Za-z0-9-_\s]+))");

                    pp.Add(p);
                }
                else if (currentParameter.ParameterType.IsEnum)
                {
                    //@"[a-z]{1}[a-z0-9]*"

                    string[] enumValues = Enum.GetNames(parameters[i].ParameterType)
                                              .Select(x => x.ToLower())
                                              .ToArray();

                    pp.Add(CreateParameter(parameters[i], string.Join("|", enumValues)));
                }
                else
                {
                    throw new Exception("parameter type is not supported: " + parameters[i].ParameterType.Name);
                }
            }

            StringBuilder builder = new StringBuilder("^");

            //function begin
            builder.Append($@"{method.Name.ToLowerInvariant()}\(");

            bool optionalParameterCall = parameters.All(x => (x.DefaultValue is DBNull) == false);

            if (optionalParameterCall)
            {
                //add optional parameters
                builder.Append($"(,|{string.Join("|", pp.Select(x => $"({x.Name}={x.Pattern})").ToArray())})*");
            }
            else
            {
                //add all parameters
                builder.Append(string.Join(",", pp.Select(x => x.Pattern).ToArray()));
            }

            //function end
            builder.Append(@"\)$");

            IFilterAction<TFilterContext> filterAction = CreateFilterAction<TFilter>(new Regex(builder.ToString(), RegexOptions.Compiled), method);

            FilterActions.Add(filterAction);
        }
    }

    /// <summary>
    /// CreateFilterAction
    /// </summary>
    /// <typeparam name="TFilter"></typeparam>
    /// <param name="regex"></param>
    /// <param name="methodInfo"></param>
    /// <returns></returns>
    protected virtual IFilterAction<TFilterContext> CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo) 
        where TFilter : TFilterBase
    {
        return new FilterAction<TFilter, TFilterContext>(ServiceProvider, regex, methodInfo);
    }

    /// <summary>
    /// Creates filter context.
    /// </summary>
    /// <returns></returns>
    protected abstract Task<TFilterContext> CreateFilterContext(PipelineContext context);

    /// <summary>
    /// Starts processing pipeline.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<DataResult> StartAsync(PipelineContext context)
    {
        using TFilterContext filterContext = await CreateFilterContext(context);

        //execute preprocessing
        PreProcessing? preProcessing = ServiceProvider.GetService<PreProcessing>();
        preProcessing?.Invoke(filterContext);

        //execute filters
        while (context.UrlFilters.Count > 0)
        {
            string filter = context.UrlFilters.Peek();

            //find and execute filter
            IFilterAction? foundFilter = FilterActions.FirstOrDefault(x => x.TryExecute(filter, filterContext));

            if (foundFilter != null)
            {
                Logger.LogTrace("Filter executed: {filter}", filter);

                filterContext.ProcessingContext.UrlFilters.Dequeue();
            }
            else
            {
                Logger.LogTrace("filter was not found: {filter}", filter);

                throw new Exception($"Filter was not found: {filter}");

                //return false;
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
