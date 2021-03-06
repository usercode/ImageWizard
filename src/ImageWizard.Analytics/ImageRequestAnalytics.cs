﻿using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Types;
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

        private object _lock = new object();

        public void OnResponseCompleted(ICachedImage cachedImage)
        {
            lock (_lock)
            {
                AnalyticsData.TransferedImages++;
                AnalyticsData.TransferedImagesInBytes += cachedImage.Metadata.FileLength;
            }
        }

        public void OnResponseSending(HttpResponse response, ICachedImage cachedImage)
        {
            
        }

        public void OnCachedImageCreated(ICachedImage cachedImage)
        {
            lock(_lock)
            {
                AnalyticsData.CreatedImages++;
                AnalyticsData.CreatedImagesInBytes += cachedImage.Metadata.FileLength;
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
