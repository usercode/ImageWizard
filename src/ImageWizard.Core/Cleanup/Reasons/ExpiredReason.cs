// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Linq.Expressions;

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
