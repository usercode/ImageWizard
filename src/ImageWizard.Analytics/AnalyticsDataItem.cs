using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Analytics
{
    public class AnalyticsDataItem
    {
        public long CachedDataSendNotModified { get; set; }

        public long CachedDataSendNotModifiedInBytes { get; set; }

        public long CachedDataSend { get; set; }

        public long CachedDataSendInBytes { get; set; }

        public long InvalidSignature { get; set; }

        public long CachedDataCreated { get; set; }

        public long CachedDataCreatedInBytes { get; set; }
    }
}
