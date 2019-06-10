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
    /// IFilterManager
    /// </summary>
    public class FilterManager
    {
        public FilterManager()
        {
            FilterActions = new List<FilterAction>();
        }

        public IList<FilterAction> FilterActions { get; set; }

        /// <summary>
        /// Register
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        public void Register<TFilter>()
            where TFilter : IFilter, new()
        {
            TFilter filter = new TFilter();

            MethodInfo[] methods = filter.GetType().GetMethods().Where(x=> x.Name == "Execute").ToArray();

            foreach(MethodInfo method in methods)
            {
                ParameterInfo[] parameters = method.GetParameters();

                StringBuilder builder = new StringBuilder();
                builder.Append($@"({filter.Name})\(");

                for (int i = 0; i < parameters.Length - 1; i++)
                {
                    if (parameters[i].ParameterType == typeof(int))
                    {
                        builder.Append($@"(?<{parameters[i].Name}>[0-9]+)");
                    }
                    else if (parameters[i].ParameterType == typeof(int))
                    {
                        builder.Append($@"(?<{parameters[i].Name}>\d+\.\d+)"); //@"-?\d+(?:\.\d+)?"
                    }
                    else if(parameters[i].ParameterType == typeof(string))
                    {
                        builder.Append($@"""(?<{parameters[i].Name}>\w+)""");
                    }
                    else if(parameters[i].ParameterType.IsEnum)
                    {
                        string[] enumValues = Enum.GetNames(parameters[i].ParameterType)
                                                    .Select(x=> x.ToLower())
                                                    .ToArray();

                        builder.Append($"(?<{parameters[i].Name}>{string.Join('|', enumValues)})");
                    }
                    else
                    {
                        throw new Exception();
                    }

                    if (i < parameters.Length - 2)
                    {
                        builder.Append(",");
                    }
                }

                builder.Append(@"\)");

                FilterAction filterAction = new FilterAction();
                filterAction.Filter = filter;
                filterAction.TargetMethod = method;
                filterAction.Regex = new Regex(builder.ToString());

                FilterActions.Add(filterAction);
            }

        }        
    }
}
