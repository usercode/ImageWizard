// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.DocNET;
using ImageWizard.DocNET.Builder;
using ImageWizard.DocNET.Filters.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

public static class DocNETBuilderExtensions
{
    public static IDocNETBuilder WithFilter<TFilter>(this IDocNETBuilder builder) 
        where TFilter : DocNETFilter
    {
        builder.Services.AddTransient<TFilter>();
        builder.Services.AddSingleton(new PipelineAction<DocNETPipeline>(x => x.AddFilter<TFilter>()));

        return builder;
    }

    //public static IDocNETBuilder WithOptions(Action<DocNETOptions> action)
    //{
    //    Builder.Services.Configure(action);

    //    return this;
    //}
}
