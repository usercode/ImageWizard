using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Analytics
{
    public class AnalyticsData : IAnalyticsData
    {
        public long TotalRequests { get; set; }

        public long SucceededRequests { get; set; }

        public long InvalidSignature { get; set; }

        public long CreatedCachedImage { get; set; }
       
    }
}
