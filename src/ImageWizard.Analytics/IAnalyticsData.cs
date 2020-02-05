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
        long TransferedImages { get; set; }
        long TransferedImagesInBytes { get; set; }

        long InvalidSignature { get; set; }

        long CreatedImages { get; set; }
        long CreatedImagesInBytes { get; set; }
    }
}
