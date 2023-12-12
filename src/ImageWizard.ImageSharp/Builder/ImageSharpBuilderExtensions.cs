// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.ImageSharp;
using ImageWizard.ImageSharp.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

public static class ImageSharpBuilderExtensions
{
    /// <summary>
    /// Registers the <typeparamref name="TFilter"/> filter.
    /// </summary>
    public static IImageSharpBuilder WithFilter<TFilter>(this IImageSharpBuilder builder) 
        where TFilter : ImageSharpFilter
    {
        builder.Services.AddTransient<TFilter>();
        builder.Services.AddSingleton(new PipelineAction<ImageSharpPipeline>(x => x.AddFilter<TFilter>()));

        return builder;
    }

    /// <summary>
    /// Sets options for the pipeline.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IImageSharpBuilder WithOptions(this IImageSharpBuilder builder, Action<ImageSharpOptions> action)
    {
        builder.Services.Configure(action);

        return builder;
    }

    /// <summary>
    /// Executes custom action before the pipeline is started.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IImageSharpBuilder WithPreProcessing(this IImageSharpBuilder builder, ImageSharpPipeline.PreProcessing action)
    {
        builder.Services.AddSingleton(action);

        return builder;
    }

    /// <summary>
    /// Executes custom action after the pipeline is finished.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IImageSharpBuilder WithPostProcessing(this IImageSharpBuilder builder, ImageSharpPipeline.PostProcessing action)
    {
        builder.Services.AddSingleton(action);

        return builder;
    }
}
