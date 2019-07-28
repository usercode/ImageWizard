using ImageWizard.Core.ImageFilters.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    /// <summary>
    /// FilterAction
    /// </summary>
    public class FilterAction<TFilter> : IFilterAction
        where TFilter : IFilter
    {
        public delegate void FilterActionHandler(GroupCollection groups, TFilter filer, FilterContext filterContext);

        public FilterAction(Regex regex, MethodInfo method, TFilter filter)
        {
            Regex = regex;
            Filter = filter;
            Method = method;

            MethodDelegate = BuildFastAccessor();
        }

        /// <summary>
        /// Regex
        /// </summary>
        public Regex Regex { get; }

        /// <summary>
        /// Filter
        /// </summary>
        public TFilter Filter { get; }

        /// <summary>
        /// TargetMethod
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// MethodDelegate
        /// </summary>
        public FilterActionHandler MethodDelegate { get; }

        public bool TryExecute(string input, FilterContext filterContext)
        {
            Match match = Regex.Match(input);

            if (match.Success == false)
            {
                return false;
            }

            MethodDelegate(match.Groups, Filter, filterContext);

            //List<object> parameterValues = new List<object>();

            //foreach(ParameterInfo pi in TargetMethod.GetParameters())
            //{
            //    string value = match.Groups[pi.Name].Value;

            //    if (pi.ParameterType == typeof(int))
            //    {
            //        parameterValues.Add(int.Parse(value, CultureInfo.InvariantCulture));
            //    }
            //    else if(pi.ParameterType == typeof(double))
            //    {
            //        parameterValues.Add(double.Parse(value, CultureInfo.InvariantCulture));
            //    }
            //    else if(pi.ParameterType == typeof(string))
            //    {
            //        parameterValues.Add(value);
            //    }
            //    else if(pi.ParameterType.IsEnum)
            //    {
            //        parameterValues.Add(Enum.Parse(pi.ParameterType, value, true));
            //    }
            //}

            //parameterValues.Add(filterContext);

            //TargetMethod.Invoke(Filter, parameterValues.ToArray());

            return true;
        }

        private FilterActionHandler BuildFastAccessor()
        {
            ParameterExpression groupsParameter = Expression.Parameter(typeof(GroupCollection));
            ParameterExpression filterParameter = Expression.Parameter(typeof(TFilter));
            ParameterExpression filterContextParameter = Expression.Parameter(typeof(FilterContext));

            //find group indexer property -> Group["Key"]
            PropertyInfo groupCollectionItemStringProperty = typeof(GroupCollection)
                                                                .GetProperties()
                                                                .FirstOrDefault(x => x.Name == "Item" && x.GetIndexParameters().Any(p => p.ParameterType == typeof(string)));

            var parsedVariables = Method.GetParameters()
                                            .Select(x =>
                                            {
                                                ParameterExpression propertyExpression = Expression.Variable(x.ParameterType, x.Name);

                                                Expression parseCall;

                                                if (x.ParameterType == typeof(int) || x.ParameterType == typeof(double))
                                                {
                                                    MethodInfo parseMethodInfo = x.ParameterType.GetMethod(nameof(int.Parse), new[] { typeof(string), typeof(CultureInfo) });
                                                    parseCall = Expression.Call(parseMethodInfo,
                                                                       Expression.Property(Expression.Property(groupsParameter, groupCollectionItemStringProperty, Expression.Constant(x.Name)), nameof(Group.Value)),
                                                                       Expression.Constant(CultureInfo.InvariantCulture));
                                                }
                                                else if (x.ParameterType.IsEnum)
                                                {
                                                    MethodInfo parseMethodInfo = typeof(Enum).GetMethod(nameof(Enum.Parse), new[] { typeof(Type), typeof(string), typeof(bool) });
                                                    parseCall = Expression.Convert(
                                                                        Expression.Call(parseMethodInfo,
                                                                               Expression.Constant(x.ParameterType),
                                                                               Expression.Property(Expression.Property(groupsParameter, groupCollectionItemStringProperty, Expression.Constant(x.Name)), nameof(Group.Value)),
                                                                               Expression.Constant(true)),
                                                                x.ParameterType);
                                                }
                                                else if (x.ParameterType == typeof(string))
                                                {
                                                    parseCall = Expression.Property(Expression.Property(groupsParameter, groupCollectionItemStringProperty, Expression.Constant(x.Name)), nameof(Group.Value));
                                                }
                                                else if (x.ParameterType == typeof(FilterContext))
                                                {
                                                    parseCall = filterContextParameter;
                                                }
                                                else
                                                {
                                                    throw new Exception("Parameter type is not supported: " + x.ParameterType.Name);
                                                }

                                                //set parsed value to variable
                                                return new { PropertyExpression = propertyExpression, AssignExpression = (Expression)Expression.Assign(propertyExpression, parseCall) };
                                            })
                                            .ToArray();

            ParameterExpression[] variables = parsedVariables
                                                .Select(x => x.PropertyExpression)
                                                .ToArray();
            Expression[] assigns = parsedVariables
                                                .Select(x => x.AssignExpression)
                                                //call filter method with parsed values
                                                .Concat(new [] { Expression.Call(
                                                                            filterParameter,
                                                                            Method,
                                                                            variables) }).ToArray();

            Expression block = Expression.Block(variables, assigns);

            return Expression.Lambda<FilterActionHandler>(block, groupsParameter, filterParameter, filterContextParameter).Compile();
        }
    }
}
