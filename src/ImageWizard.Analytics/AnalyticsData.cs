using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Analytics
{
    public class AnalyticsData : IAnalyticsData
    {
        public long TransferedImages { get; set; }

        public long TransferedImagesInBytes { get; set; }

        public long InvalidSignature { get; set; }

        public long CreatedImages { get; set; }

        public long CreatedImagesInBytes { get; set; }
    }
}
