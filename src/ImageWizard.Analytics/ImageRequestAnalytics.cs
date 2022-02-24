﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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
        public ImageRequestAnalytics(AnalyticsData analyticsData)
        {
            AnalyticsData = analyticsData;
        }

        /// <summary>
        /// AnalyticsData
        /// </summary>
        private AnalyticsData AnalyticsData { get; }

        private readonly object _lock = new object();

        private void SetData(string? mimeType, Action<AnalyticsDataItem> action)
        {
            lock (_lock)
            {
                //total stats
                action(AnalyticsData.Total);

                //stats by mime type
                mimeType ??= string.Empty;

                if (AnalyticsData.ByMimeType.TryGetValue(mimeType, out AnalyticsDataItem? value) == false)
                {
                    value = new AnalyticsDataItem();

                    AnalyticsData.ByMimeType.Add(mimeType, value);
                }

                action(value);
            }
        }

        public void OnCachedDataSending(HttpResponse response, ICachedData cachedData, bool notModified)
        {
            SetData(cachedData.Metadata.MimeType, x =>
            {
                if (notModified)
                {
                    x.CachedDataSendNotModified++;
                    x.CachedDataSendNotModifiedInBytes += cachedData.Metadata.FileLength;
                }
                else
                {
                    x.CachedDataSend++;
                    x.CachedDataSendInBytes += cachedData.Metadata.FileLength;
                }
            });
        }

        public void OnCachedDataCreated(ICachedData cachedData)
        {
            SetData(cachedData.Metadata.MimeType, x =>
            {
                x.CachedDataCreated++;
                x.CachedDataCreatedInBytes += cachedData.Metadata.FileLength;
            });
        }

        public void OnUnsafeSignature()
        {
            SetData(null, x =>
            {
                x.UnsafeSignature++;
            });
        }

        public void OnValidSignature()
        {
            SetData(null, x =>
            {
                x.ValidSignature++;
            });
        }

        public void OnInvalidSignature()
        {
            SetData(null, x =>
            {
                x.InvalidSignature++;
            });
        }
    }
}
