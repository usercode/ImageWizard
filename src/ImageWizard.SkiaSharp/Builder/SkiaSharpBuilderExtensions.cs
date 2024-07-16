// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.SkiaSharp;
using ImageWizard.SkiaSharp.Builder;
using ImageWizard.SkiaSharp.Filters.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

public static class SkiaSharpBuilderExtensions
{
    public static ISkiaSharpBuilder WithFilter<TFilter>(this ISkiaSharpBuilder builder) 
        where TFilter : SkiaSharpFilter, IFilterFactory
    {
        builder.Services.AddTransient<TFilter>();
        builder.Services.AddSingleton(new PipelineAction<SkiaSharpPipeline>(x => x.AddFilter<TFilter>()));

        return builder;
    }

    public static ISkiaSharpBuilder WithOptions(this ISkiaSharpBuilder builder, Action<SkiaSharpOptions> action)
    {
        builder.Services.Configure(action);

        return builder;
    }
}
