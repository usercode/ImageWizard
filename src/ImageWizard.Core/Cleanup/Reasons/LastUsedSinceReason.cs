// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Caches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Cleanup;

/// <summary>
/// Removes cached data which are last used since defined duration.
/// </summary>
public class LastUsedSinceReason : CleanupReason
{
    public LastUsedSinceReason(TimeSpan duration)
    {
        Duration = duration;
    }

    /// <summary>
    /// Name
    /// </summary>
    public override string Name => $"Last used since {Duration}";

    /// <summary>
    /// Duration
    /// </summary>
    public TimeSpan Duration { get; }

    private DateTime MinDate => DateTime.UtcNow - Duration;

    public override bool CanUse(ICache cache)
    {
        return cache is ILastAccessCache;
    }

    public override Expression<Func<T, bool>> GetExpression<T>()
    {
        return x => x.LastAccess < MinDate;
    }
}
