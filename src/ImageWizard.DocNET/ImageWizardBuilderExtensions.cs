// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Processing.Builder;
using ImageWizard.DocNET;
using ImageWizard.DocNET.Builder;
using ImageWizard.DocNET.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard;

/// <summary>
/// FilterExtensions
/// </summary>
public static class ImageWizardBuilderExtensions
{
    public static IImageWizardBuilder AddDocNET(this IImageWizardBuilder builder)
    {
        return AddDocNET(builder, x => x.WithMimeTypes(MimeTypes.Pdf));
    }

    public static IImageWizardBuilder AddDocNET(this IImageWizardBuilder builder, Action<IDocNETBuilder> options)
    {
        DocNETBuilder pipelineBuilder = new DocNETBuilder(builder.Services);

        pipelineBuilder.WithFilter<PageToImageFilter>();
        pipelineBuilder.WithFilter<SubPagesFilter>();

        pipelineBuilder.WithMimeTypes(MimeTypes.Pdf);

        options(pipelineBuilder);

        builder.AddPipeline<DocNETPipeline>(pipelineBuilder.MimeTypes);

        return builder;
    }
}
