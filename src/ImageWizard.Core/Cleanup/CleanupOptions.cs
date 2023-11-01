// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Cleanup;

/// <summary>
/// CleanupOptions
/// </summary>
public class CleanupOptions
{
    /// <summary>
    /// Cleanup reasons
    /// </summary>
    public IList<CleanupReason> Reasons { get; } = new List<CleanupReason>();

    /// <summary>
    /// Duration between the cleanup actions. (Default: 1 day)
    /// </summary>
    public TimeSpan Interval { get; set; } = TimeSpan.FromDays(1);
}
