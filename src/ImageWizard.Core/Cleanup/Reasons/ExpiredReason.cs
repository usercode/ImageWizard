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
/// Removes cached data which are expired.
/// </summary>
public class ExpiredReason : CleanupReason
{
    public ExpiredReason()
    {
    }

    public override string Name => "Expired";

    public override Expression<Func<T, bool>> GetExpression<T>()
    {
        return x => x.Cache.Expires < DateTime.UtcNow;
    }
}
