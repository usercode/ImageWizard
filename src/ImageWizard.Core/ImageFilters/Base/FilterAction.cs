using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
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
    public abstract class FilterAction<TFilter> : IFilterAction
        where TFilter : IFilter
    {
        public delegate void FilterActionHandler(GroupCollection groups, TFilter filer);

        public FilterAction(IServiceProvider serviceProvider, Regex regex, MethodInfo method)
        {
            ServiceProvider = serviceProvider;
            Name = method.Name;
            Regex = regex;

            MethodDelegate = BuildFastAccessor(method);
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ServiceProvider
        /// </summary>
        private IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Regex
        /// </summary>
        private Regex Regex { get; }

        /// <summary>
        /// MethodDelegate
        /// </summary>
        private FilterActionHandler MethodDelegate { get; }

        public bool TryExecute(string input, FilterContext filterContext)
        {
            Match match = Regex.Match(input);

            if (match.Success == false)
            {
                return false;
            }

            TFilter filter = ServiceProvider.GetRequiredService<TFilter>();
            filter.Context = filterContext;

            MethodDelegate(match.Groups, filter);

            return true;
        }

        private FilterActionHandler BuildFastAccessor(MethodInfo method)
        {
            ParameterExpression groupsParameter = Expression.Parameter(typeof(GroupCollection));
            ParameterExpression filterParameter = Expression.Parameter(typeof(TFilter));

            //find group indexer property -> Group["Key"]
            PropertyInfo groupCollectionItemStringProperty = typeof(GroupCollection)
                                                                .GetProperties()
                                                                .FirstOrDefault(x => x.Name == "Item" && x.GetIndexParameters().Any(p => p.ParameterType == typeof(string)));

            var parsedVariables = method.GetParameters()
                                            .Select(x =>
                                            {
                                                ParameterExpression propertyExpression = Expression.Variable(x.ParameterType, x.Name);

                                                Expression groupSuccess = Expression.Property(Expression.Property(groupsParameter, groupCollectionItemStringProperty, Expression.Constant(x.Name)), nameof(Group.Success));
                                                Expression groupValue = Expression.Property(Expression.Property(groupsParameter, groupCollectionItemStringProperty, Expression.Constant(x.Name)), nameof(Group.Value));
                                                Expression defaultValue = x.DefaultValue is DBNull ? (Expression)Expression.Default(x.ParameterType) : (Expression)Expression.Constant(x.DefaultValue);

                                                Expression result = null;
                                                Expression parsedValue = null;

                                                if (x.ParameterType == typeof(bool))
                                                {
                                                    MethodInfo parseMethodInfo = x.ParameterType.GetMethod(nameof(bool.Parse), new[] { typeof(string) });

                                                    parsedValue = Expression.Call(
                                                                                parseMethodInfo, 
                                                                                groupValue);
                                                }
                                                else if (x.ParameterType.IsPrimitive)
                                                {
                                                    MethodInfo parseMethodInfo = x.ParameterType.GetMethod(nameof(int.Parse), new[] { typeof(string), typeof(CultureInfo) });

                                                    parsedValue = Expression.Call(
                                                                               parseMethodInfo,
                                                                               groupValue,
                                                                               Expression.Constant(CultureInfo.InvariantCulture));
                                                }
                                                else if (x.ParameterType.IsEnum)
                                                {
                                                    MethodInfo parseMethodInfo = typeof(Enum).GetMethod(nameof(Enum.Parse), new[] { typeof(Type), typeof(string), typeof(bool) });

                                                    parsedValue = Expression.Convert(
                                                                                Expression.Call(
                                                                                        parseMethodInfo,
                                                                                        Expression.Constant(x.ParameterType),
                                                                                        groupValue,
                                                                                        Expression.Constant(true)), 
                                                                               x.ParameterType);
                                                }
                                                else if (x.ParameterType == typeof(string))
                                                {
                                                    parsedValue = groupValue;
                                                }
                                                else
                                                {
                                                    throw new Exception("Parameter type is not supported: " + x.ParameterType.Name);
                                                }

                                                result = Expression.Condition(
                                                                            groupSuccess,
                                                                            parsedValue,
                                                                            defaultValue
                                                                            );                                                

                                                List<Expression> results = new List<Expression>();

                                                results.Add(Expression.Assign(propertyExpression, result));

                                                AppliedPropertyExpression(x, filterParameter, propertyExpression, results);

                                                //check dpr attribute
                                                if (x.GetCustomAttribute<DPRAttribute>() != null)
                                                {
                                                    //multiply parameter with dpr value
                                                    results.Add(
                                                    Expression.IfThen(Expression.NotEqual(Expression.Property(Expression.Property(Expression.Property(Expression.Property(filterParameter, nameof(IFilter.Context)), nameof(FilterContext.ProcessingContext)), nameof(ProcessingPipelineContext.ClientHints)), nameof(ClientHints.DPR)), Expression.Constant(null)),
                                                    Expression.Assign(propertyExpression,
                                                        Expression.Convert(
                                                            Expression.Multiply(Expression.Convert(propertyExpression, typeof(double)), Expression.Property(Expression.Property(Expression.Property(Expression.Property(Expression.Property(filterParameter, nameof(IFilter.Context)), nameof(FilterContext.ProcessingContext)), nameof(ProcessingPipelineContext.ClientHints)), nameof(ClientHints.DPR)), nameof(Nullable<double>.Value))),
                                                            x.ParameterType))));
                                                }

                                                //set parsed value to variable
                                                return new { PropertyExpression = propertyExpression, AssignExpression = results.ToArray() };
                                            })
                                            .ToArray();

            ParameterExpression[] variables = parsedVariables
                                                .Select(x => x.PropertyExpression)
                                                .ToArray();
            Expression[] assigns = parsedVariables
                                                .SelectMany(x => x.AssignExpression)
                                                //call filter method with parsed values
                                                .Concat(new[] { Expression.Call(
                                                                            filterParameter,
                                                                            method,
                                                                            variables) }).ToArray();

            Expression block = Expression.Block(variables, assigns);

            return Expression.Lambda<FilterActionHandler>(block, groupsParameter, filterParameter).Compile();
        }

        protected virtual void AppliedPropertyExpression(ParameterInfo parameterInfo, ParameterExpression filterParameter, ParameterExpression propertyExpr, IList<Expression> result)
        {

        }

       
    }
}
