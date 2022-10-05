// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.SvgNet.Builder;
using ImageWizard.SvgNet.Filters;
using System;

namespace ImageWizard;

/// <summary>
/// FilterExtensions
/// </summary>
public static class ImageWizardBuilderExtensions
{
    public static IImageWizardBuilder AddSvgNet(this IImageWizardBuilder builder, Action<ISvgNetBuilder>? options = null)
    {
        SvgNetBuilder pipelineBuilder = new SvgNetBuilder(builder.Services);

        pipelineBuilder.WithFilter<RemoveSizeFilter>();
        pipelineBuilder.WithFilter<RotateFilter>();
        pipelineBuilder.WithFilter<BlurFilter>();
        pipelineBuilder.WithFilter<GrayscaleFilter>();
        pipelineBuilder.WithFilter<InvertFilter>();
        pipelineBuilder.WithFilter<SaturateFilter>();
        pipelineBuilder.WithFilter<ImageFormatFilter>();

        pipelineBuilder.WithMimeTypes(MimeTypes.Svg);

        options?.Invoke(pipelineBuilder);

        builder.AddPipeline<SvgPipeline>(pipelineBuilder.MimeTypes);

        return builder;
    }
}
