// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Analytics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace ImageWizard;

public static class Extensions
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions() { WriteIndented = true };

    public static IImageWizardBuilder AddAnalytics(this IImageWizardBuilder builder)
    {
        builder.Services.AddSingleton<AnalyticsData>();
        builder.Services.AddSingleton<IImageWizardInterceptor, ImageRequestAnalytics>();

        return builder;
    }

    public static IEndpointConventionBuilder MapAnalytics(this IImageWizardEndpointBuilder endpoints)
    {
        return endpoints.MapGet("analytics", (AnalyticsData data) => Results.Json(data, JsonOptions));
    }
}
