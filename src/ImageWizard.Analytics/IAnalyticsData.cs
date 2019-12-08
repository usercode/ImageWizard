using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Analytics
{
    /// <summary>
    /// IAnalyticsData
    /// </summary>
    public interface IAnalyticsData
    {
        long TotalRequests { get; set; }

        long SucceededRequests { get; set; }

        long InvalidSignature { get; set; }

        long CreatedCachedImage { get; set; }
    }
}
