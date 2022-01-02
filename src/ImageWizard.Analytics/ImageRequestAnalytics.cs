using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Analytics
{
    /// <summary>
    /// ImageRequestAnalytics
    /// </summary>
    class ImageRequestAnalytics : IImageWizardInterceptor
    {
        public ImageRequestAnalytics(IAnalyticsData analyticsData)
        {
            AnalyticsData = analyticsData;
        }

        /// <summary>
        /// AnalyticsData
        /// </summary>
        private IAnalyticsData AnalyticsData { get; }

        private readonly object _lock = new object();

        public void OnResponseSending(HttpResponse response, ICachedData cachedImage)
        {
            lock (_lock)
            {
                AnalyticsData.TransferedImages++;                
                AnalyticsData.TransferedImagesInBytes += cachedImage.Metadata.FileLength ?? 0;
            }
        }

        public void OnCachedImageCreated(ICachedData cachedImage)
        {
            lock(_lock)
            {
                AnalyticsData.CreatedImages++;
                AnalyticsData.CreatedImagesInBytes += cachedImage.Metadata.FileLength ?? 0;
            }
        }

        public void OnFailedSignature()
        {
            lock(_lock)
            {
                AnalyticsData.InvalidSignature++;
            }
        }
    }
}
