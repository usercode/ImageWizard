// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.SkiaSharp;
using ImageWizard.SkiaSharp.Builder;
using ImageWizard.SkiaSharp.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard;

public static class ImageWizardBuilderExtensions
{
    private readonly static string[] SupportedMimeTypes = new[] { MimeTypes.WebP, MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bmp };

    public static IImageWizardBuilder AddSkiaSharp(this IImageWizardBuilder builder)
    {
        return AddSkiaSharp(builder, SupportedMimeTypes);
    }

    public static IImageWizardBuilder AddSkiaSharp(this IImageWizardBuilder builder, params string[] mimeTypes)
    {
        return AddSkiaSharp(builder, x => x.WithMimeTypes(mimeTypes));
    }

    public static IImageWizardBuilder AddSkiaSharp(this IImageWizardBuilder builder,Action<ISkiaSharpBuilder> options)
    {
        SkiaSharpBuilder pipelineBuilder = new SkiaSharpBuilder(builder.Services);

        pipelineBuilder.WithFilter<ResizeFilter>();
        pipelineBuilder.WithFilter<RotateFilter>();
        pipelineBuilder.WithFilter<CropFilter>();
        pipelineBuilder.WithFilter<GrayscaleFilter>();
        pipelineBuilder.WithFilter<BlurFilter>();
        pipelineBuilder.WithFilter<FlipFilter>();
        pipelineBuilder.WithFilter<DPRFilter>();
        pipelineBuilder.WithFilter<ImageFormatFilter>();
        pipelineBuilder.WithFilter<TextFilter>();

        pipelineBuilder.WithMimeTypes(SupportedMimeTypes);

        options(pipelineBuilder);

        builder.AddPipeline<SkiaSharpPipeline>(pipelineBuilder.MimeTypes);

        return builder;
    }
}
