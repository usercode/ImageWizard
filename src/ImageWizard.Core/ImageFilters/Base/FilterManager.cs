using ImageWizard.Core.ImageFilters.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    /// <summary>
    /// FilterManager
    /// </summary>
    public class FilterManager
    {
        public FilterManager()
        {
            FilterActions = new List<IFilterAction>();
        }

        public IList<IFilterAction> FilterActions { get; set; }

        /// <summary>
        /// Register
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        public void Register<TFilter>()
            where TFilter : IFilter, new()
        {
            TFilter filter = new TFilter();

            MethodInfo[] methods = filter.GetType()
                                            .GetMethods()
                                            .Where(x=> x.Name == "Execute")
                                            .ToArray();

            foreach(MethodInfo method in methods)
            {
                ParameterInfo[] parameters = method.GetParameters();

                StringBuilder builder = new StringBuilder("^");
                builder.Append($@"{filter.Name}\(");

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i].ParameterType == typeof(int))
                    {
                        builder.Append($@"(?<{parameters[i].Name}>\d+)");
                    }
                    else if (parameters[i].ParameterType == typeof(double))
                    {
                        builder.Append($@"(?<{parameters[i].Name}>\d+\.\d+)");
                    }
                    else if(parameters[i].ParameterType == typeof(string))
                    {
                        builder.Append($@"'(?<{parameters[i].Name}>.+)'");
                    }
                    else if(parameters[i].ParameterType.IsEnum)
                    {
                        string[] enumValues = Enum.GetNames(parameters[i].ParameterType)
                                                    .Select(x=> x.ToLower())
                                                    .ToArray();

                        builder.Append($"(?<{parameters[i].Name}>{string.Join("|", enumValues)})");
                    }
                    else if(parameters[i].ParameterType == typeof(FilterContext))
                    {
                        //skip this parameter
                    }
                    else
                    {
                        throw new Exception("parameter type is not supported: " + parameters[i].ParameterType.Name);
                    }

                    if (i < parameters.Length - 2)
                    {
                        builder.Append(",");
                    }
                }

                builder.Append(@"\)$");

                FilterAction<TFilter> filterAction = new FilterAction<TFilter>(
                                                    new Regex(builder.ToString(), RegexOptions.Compiled),
                                                    method,
                                                    filter);

                FilterActions.Add(filterAction);
            }
        }        
    }
}
