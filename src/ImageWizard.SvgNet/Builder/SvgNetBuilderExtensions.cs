﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Filters;
using ImageWizard.Processing;
using ImageWizard.SvgNet;
using ImageWizard.SvgNet.Builder;
using ImageWizard.SvgNet.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

public static class SvgNetBuilderExtensions
{
    public static ISvgNetBuilder WithFilter<TFilter>(this ISvgNetBuilder builder) 
        where TFilter : SvgFilter, IFilterFactory
    {
        builder.Services.AddTransient<TFilter>();
        builder.Services.AddSingleton(new PipelineAction<SvgPipeline>(x => x.AddFilter<TFilter>()));

        return builder;
    }

    public static ISvgNetBuilder WithOptions(this ISvgNetBuilder builder, Action<SvgOptions> action)
    {
        builder.Services.Configure(action);

        return builder;
    }
}
