// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Processing.Builder;
using ImageWizard.ImageSharp.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard;

/// <summary>
/// FilterExtensions
/// </summary>
public static class ImageWizardBuilderExtensions
{
    /// <summary>
    /// Adds the ImageSharp pipeline with custom options.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IImageWizardBuilder AddImageSharp(this IImageWizardBuilder builder, Action<IImageSharpBuilder>? options = null)
    {
        ImageSharpBuilder pipelineBuilder = new ImageSharpBuilder(builder.Services);

        pipelineBuilder.WithFilter<ResizeFilter>();
        pipelineBuilder.WithFilter<BackgroundColorFilter>();
        pipelineBuilder.WithFilter<CropFilter>();
        pipelineBuilder.WithFilter<GrayscaleFilter>();
        pipelineBuilder.WithFilter<BlackWhiteFilter>();
        pipelineBuilder.WithFilter<TrimFilter>();
        pipelineBuilder.WithFilter<FlipFilter>();
        pipelineBuilder.WithFilter<RotateFilter>();
        pipelineBuilder.WithFilter<BlurFilter>();
        pipelineBuilder.WithFilter<InvertFilter>();
        pipelineBuilder.WithFilter<BrightnessFilter>();
        pipelineBuilder.WithFilter<ContrastFilter>();
        pipelineBuilder.WithFilter<DPRFilter>();
        pipelineBuilder.WithFilter<AutoOrientFilter>();
        pipelineBuilder.WithFilter<MetadataFilter>();
        pipelineBuilder.WithFilter<ImageFormatFilter>();
        pipelineBuilder.WithFilter<TextFilter>();

        pipelineBuilder.WithMimeTypes(new[] { MimeTypes.WebP, MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Tga, MimeTypes.Bmp });

        options?.Invoke(pipelineBuilder);

        builder.AddPipeline<ImageSharpPipeline>(pipelineBuilder.MimeTypes);

        return builder;
    }
}
