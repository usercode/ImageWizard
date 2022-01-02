using Microsoft.Extensions.DependencyInjection;
using System;

namespace ImageWizard.Analytics
{
    public static class Extensions
    {
        public static IImageWizardBuilder AddAnalytics(this IImageWizardBuilder builder)
        {
            builder.Services.AddSingleton<IAnalyticsData, AnalyticsData>();
            builder.Services.AddTransient<IImageWizardInterceptor, ImageRequestAnalytics>();

            return builder;
        }
    }
}
