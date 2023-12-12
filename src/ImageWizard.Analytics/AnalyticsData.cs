// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Analytics;

/// <summary>
/// AnalyticsData
/// </summary>
public class AnalyticsData
{
    public AnalyticsData()
    {
        Total = new AnalyticsDataItem();
        ByMimeType = new Dictionary<string, AnalyticsDataItem>();
    }

    /// <summary>
    /// Total
    /// </summary>
    public AnalyticsDataItem Total { get; set; }

    /// <summary>
    /// ByMimeType
    /// </summary>
    public IDictionary<string, AnalyticsDataItem> ByMimeType { get; }
}
