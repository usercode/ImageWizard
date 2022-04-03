// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Cleanup;

/// <summary>
/// CleanupOptions
/// </summary>
public class CleanupOptions
{
    public CleanupOptions()
    {
        Interval = TimeSpan.FromDays(1);
        Reasons = new List<CleanupReason>();
    }

    /// <summary>
    /// Reasons
    /// </summary>
    public IList<CleanupReason> Reasons { get; }

    /// <summary>
    /// Duration between the cleanup actions. (Default: 1 day)
    /// </summary>
    public TimeSpan Interval { get; set; }

    
}
