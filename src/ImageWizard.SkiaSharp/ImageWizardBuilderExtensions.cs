// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.SkiaSharp;
using ImageWizard.SkiaSharp.Builder;
using ImageWizard.SkiaSharp.Filters;

namespace ImageWizard;

public static class ImageWizardBuilderExtensions
{
    public static IImageWizardBuilder AddSkiaSharp(this IImageWizardBuilder builder, Action<ISkiaSharpBuilder>? options = null)
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

        pipelineBuilder.WithMimeTypes(new[] { MimeTypes.WebP, MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bmp });

        options?.Invoke(pipelineBuilder);

        builder.AddPipeline<SkiaSharpPipeline>(pipelineBuilder.MimeTypes);

        return builder;
    }
}
