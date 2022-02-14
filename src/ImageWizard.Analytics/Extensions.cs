using ImageWizard.Analytics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace ImageWizard
{
    public static class Extensions
    {
        private static JsonSerializerOptions JsonOptions = new JsonSerializerOptions() { WriteIndented = true };

        public static IImageWizardBuilder AddAnalytics(this IImageWizardBuilder builder)
        {
            builder.Services.AddSingleton<AnalyticsData>();
            builder.Services.AddSingleton<IImageWizardInterceptor, ImageRequestAnalytics>();

            return builder;
        }

        public static void MapAnalytics(this IImageWizardEndpointBuilder endpoints)
        {
            endpoints.MapGet("analytics", (AnalyticsData data) => Results.Json(data, JsonOptions));
        }
    }
}
