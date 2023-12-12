// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.DocNET;
using ImageWizard.DocNET.Builder;
using ImageWizard.DocNET.Filters;

namespace ImageWizard;

/// <summary>
/// FilterExtensions
/// </summary>
public static class ImageWizardBuilderExtensions
{
    public static IImageWizardBuilder AddDocNET(this IImageWizardBuilder builder, Action<IDocNETBuilder>? options = null)
    {
        DocNETBuilder pipelineBuilder = new DocNETBuilder(builder.Services);

        pipelineBuilder.WithFilter<PageToImageFilter>();
        pipelineBuilder.WithFilter<SubPagesFilter>();

        pipelineBuilder.WithMimeTypes(MimeTypes.Pdf);

        options?.Invoke(pipelineBuilder);

        builder.AddPipeline<DocNETPipeline>(pipelineBuilder.MimeTypes);

        return builder;
    }
}
