// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.SkiaSharp;
using ImageWizard.SkiaSharp.Builder;
using ImageWizard.SkiaSharp.Filters.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

public static class SkiaSharpBuilderExtensions
{
    public static ISkiaSharpBuilder WithFilter<TFilter>(this ISkiaSharpBuilder builder) where TFilter : SkiaSharpFilter, new()
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
