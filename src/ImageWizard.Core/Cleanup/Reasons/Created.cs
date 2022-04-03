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

public class Created : CleanupReason
{
    public Created(TimeSpan duration)
    {
        Duration = duration;
    }

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
