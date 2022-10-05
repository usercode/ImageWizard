// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.Processing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageWizard;

/// <summary>
/// FilterAction
/// </summary>
public class FilterAction<TFilter, TFilterContext> : IFilterAction<TFilterContext>
    where TFilter : IFilter<TFilterContext>
    where TFilterContext : FilterContext
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

    public bool TryExecute(string input, TFilterContext filterContext)
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
        PropertyInfo? groupCollectionItemStringProperty = typeof(GroupCollection)
                                                            .GetProperties()
                                                            .FirstOrDefault(x => x.Name == "Item" && x.GetIndexParameters().Any(p => p.ParameterType == typeof(string)));

        if (groupCollectionItemStringProperty == null)
        {
            throw new ArgumentNullException(nameof(groupCollectionItemStringProperty));
        }

        var parsedVariables = method.GetParameters()
                                        .Select(x =>
                                        {
                                            ParameterExpression propertyExpression = Expression.Variable(x.ParameterType, x.Name);

                                            Expression groupSuccess = Expression.Property(Expression.Property(groupsParameter, groupCollectionItemStringProperty, Expression.Constant(x.Name)), nameof(Group.Success));
                                            Expression groupValue = Expression.Property(Expression.Property(groupsParameter, groupCollectionItemStringProperty, Expression.Constant(x.Name)), nameof(Group.Value));
                                            Expression defaultValue = x.DefaultValue is DBNull ? (Expression)Expression.Default(x.ParameterType) : (Expression)Expression.Constant(x.DefaultValue);

                                            Expression? result = null;
                                            Expression? parsedValue = null;

                                            if (x.ParameterType == typeof(bool))
                                            {
                                                MethodInfo? parseMethodInfo = x.ParameterType.GetMethod(nameof(bool.Parse), new[] { typeof(string) });

                                                if (parseMethodInfo == null)
                                                {
                                                    throw new ArgumentNullException(nameof(parseMethodInfo));
                                                }

                                                parsedValue = Expression.Call(
                                                                            parseMethodInfo, 
                                                                            groupValue);
                                            }
                                            else if (x.ParameterType.IsPrimitive)
                                            {
                                                MethodInfo? parseMethodInfo = x.ParameterType.GetMethod(nameof(int.Parse), new[] { typeof(string), typeof(CultureInfo) });

                                                if (parseMethodInfo == null)
                                                {
                                                    throw new ArgumentNullException(nameof(parseMethodInfo));
                                                }

                                                parsedValue = Expression.Call(
                                                                           parseMethodInfo,
                                                                           groupValue,
                                                                           Expression.Constant(CultureInfo.InvariantCulture));
                                            }
                                            else if (x.ParameterType.IsEnum)
                                            {
                                                MethodInfo? parseMethodInfo = typeof(Enum).GetMethod(nameof(Enum.Parse), new[] { typeof(Type), typeof(string), typeof(bool) });

                                                if (parseMethodInfo == null)
                                                {
                                                    throw new ArgumentNullException(nameof(parseMethodInfo));
                                                }

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
                                                MethodInfo? startsWith = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(char) });
                                                MethodInfo? subString = typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) });

                                                if (startsWith == null)
                                                {
                                                    throw new ArgumentNullException(nameof(startsWith));
                                                }

                                                if (subString == null)
                                                {
                                                    throw new ArgumentNullException(nameof(subString));
                                                }

                                                MethodInfo? base64Decode = typeof(WebEncoders).GetMethod(nameof(WebEncoders.Base64UrlDecode), new[] { typeof(string) });
                                                MethodInfo? getBytes = typeof(Encoding).GetMethod(nameof(Encoding.GetString), new[] { typeof(byte[]) });
                                                PropertyInfo? utf8 = typeof(Encoding).GetProperty(nameof(Encoding.UTF8));

                                                if (base64Decode == null)
                                                {
                                                    throw new ArgumentNullException(nameof(base64Decode));
                                                }

                                                if (getBytes == null)
                                                {
                                                    throw new ArgumentNullException(nameof(getBytes));
                                                }

                                                if (utf8 == null)
                                                {
                                                    throw new ArgumentNullException(nameof(utf8));
                                                }

                                                //raw string or base54url?
                                                parsedValue = Expression.Condition(
                                                                    Expression.Call(groupValue, startsWith, Expression.Constant('\'')),
                                                                    Expression.Call(groupValue, subString, 
                                                                        Expression.Constant(1), 
                                                                        Expression.Subtract(Expression.Property(groupValue, nameof(string.Length)), Expression.Constant(2))),
                                                                    Expression.Call(Expression.Property(null, utf8), getBytes, Expression.Call(base64Decode, groupValue)));
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
                                                Expression.IfThen(Expression.NotEqual(Expression.Property(Expression.Property(Expression.Property(Expression.Property(filterParameter, nameof(IFilter<TFilterContext>.Context)), nameof(FilterContext.ProcessingContext)), nameof(PipelineContext.ClientHints)), nameof(ClientHints.DPR)), Expression.Constant(null)),
                                                Expression.Assign(propertyExpression,
                                                    Expression.Convert(
                                                        Expression.Multiply(Expression.Convert(propertyExpression, typeof(double)), Expression.Property(Expression.Property(Expression.Property(Expression.Property(Expression.Property(filterParameter, nameof(IFilter<TFilterContext>.Context)), nameof(FilterContext.ProcessingContext)), nameof(PipelineContext.ClientHints)), nameof(ClientHints.DPR)), nameof(Nullable<double>.Value))),
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
