// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Cleanup;

/// <summary>
/// CleanupReason
/// </summary>
public abstract class CleanupReason
{
    public CleanupReason()
    {

    }

    private IDictionary<Type, object> _cache = new Dictionary<Type, object>();

    /// <summary>
    /// GetExpression
    /// </summary>
    public abstract Expression<Func<T, bool>> GetExpression<T>() where T : IMetadata;

    /// <summary>
    /// IsValid
    /// </summary>
    /// <param name="cachedData"></param>
    /// <returns></returns>
    public bool IsValid<T>(T cachedData) where T : IMetadata
    {
        if (_cache.TryGetValue(typeof(T), out object? result) == false)
        {
            result = GetExpression<T>().Compile();

            _cache.Add(typeof(T), result);
        }

        Func<T, bool> method = (Func<T, bool>)result;

        return method(cachedData);
    }
}
