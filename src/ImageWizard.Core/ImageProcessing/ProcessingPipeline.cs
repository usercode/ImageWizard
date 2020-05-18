using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Core.ImageFilters.Base.Helpers;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    /// <summary>
    /// ProcessingPipeline
    /// </summary>
    public abstract class ProcessingPipeline<TFilterBase> : IProcessingPipeline
        where TFilterBase : IFilter
    {
        public ProcessingPipeline()
        {
            FilterActions = new List<IFilterAction>();
        }

        protected abstract IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo) where TFilter : TFilterBase, new();

        public IList<IFilterAction> FilterActions { get; set; }

        /// <summary>
        /// AddFilter
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        public void AddFilter<TFilter>()
            where TFilter : TFilterBase, new()
        {
            MethodInfo[] methods = typeof(TFilter)
                                        .GetMethods()
                                        .Where(x => x.IsPublic)
                                        .Where(x=> x.GetCustomAttribute<FilterAttribute>() != null)
                                        .ToArray();

            foreach(MethodInfo method in methods)
            {
                ParameterInfo[] parameters = method.GetParameters();

                Type[] integerTypes = new Type[] { typeof(byte), typeof(short), typeof(int), typeof(long) };
                Type[] floatingNumberTypes = new Type[] { typeof(float), typeof(double), typeof(decimal) };

                ParameterItem CreateParameter(ParameterInfo pi, string pattern)
                {
                    return new ParameterItem() { Name = pi.Name, Pattern = $@"(?<{pi.Name}>{pattern})" };
                }

                List<ParameterItem> pp = new List<ParameterItem>();

                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo currentParameter = parameters[i];

                    if (integerTypes.Any(x => x == currentParameter.ParameterType))
                    {
                        pp.Add(CreateParameter(currentParameter, @"\d+"));
                    }
                    else if (floatingNumberTypes.Any(x => x == currentParameter.ParameterType))
                    {
                        pp.Add(CreateParameter(currentParameter, @"\d+\.\d+"));
                    }
                    else if (currentParameter.ParameterType == typeof(bool))
                    {
                        pp.Add(CreateParameter(parameters[i], "True|False"));
                    }
                    else if (currentParameter.ParameterType == typeof(string))
                    {
                        ParameterItem p = CreateParameter(parameters[i], ".+");

                        p.Pattern = $"'{p.Pattern}'";

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

                bool optionalParmeterCall = parameters.All(x => (x.DefaultValue is DBNull) == false);

                if (optionalParmeterCall)
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

                IFilterAction filterAction = CreateFilterAction<TFilter>(new Regex(builder.ToString(), RegexOptions.Compiled), method);

                FilterActions.Add(filterAction);
            }
        }

        protected bool ProcessFilters(IEnumerable<string> urlFilters, FilterContext filterContext)
        {
            //execute filters
            foreach (string filter in urlFilters)
            {
                //find and execute filter
                IFilterAction foundFilter = FilterActions.FirstOrDefault(x => x.TryExecute(filter, filterContext));

                if (foundFilter != null)
                {
                    //Logger.LogTrace("Filter executed: " + filter);
                }
                else
                {
                    //Logger.LogTrace($"filter was not found: {filter}");

                    return false;
                }
            }

            return true;
        }

        public abstract Task StartAsync(ProcessingPipelineContext context);
    }
}
