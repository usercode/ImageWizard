// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Linq.Expressions;

namespace ImageWizard.Cleanup;

/// <summary>
/// Removes cached data which are older than defined duration.
/// </summary>
public class OlderThanReason : CleanupReason
{
    public OlderThanReason(TimeSpan duration)
    {
        Duration = duration;
    }

    public override string Name => $"Older than {Duration}";

    /// <summary>
    /// Duration
    /// </summary>
    public TimeSpan Duration { get; }

    private DateTime MinDate => DateTime.UtcNow - Duration;

    public override Expression<Func<T, bool>> GetExpression<T>()
    {
        return x => x.Created < MinDate;
    }
}
