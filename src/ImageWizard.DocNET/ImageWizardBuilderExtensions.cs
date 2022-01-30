using ImageWizard.Core.Processing.Builder;
using ImageWizard.DocNET;
using ImageWizard.DocNET.Builder;
using ImageWizard.DocNET.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// FilterExtensions
    /// </summary>
    public static class ImageWizardBuilderExtensions
    {
        public static IImageWizardBuilder AddDocNET(this IImageWizardBuilder builder)
        {
            return AddDocNET(builder, MimeTypes.Pdf);
        }

        public static IImageWizardBuilder AddDocNET(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            return AddDocNET(builder, x => x.WithMimeTypes(mimeTypes));
        }

        public static IImageWizardBuilder AddDocNET(this IImageWizardBuilder builder, Action<IDocNETBuilder> options)
        {
            DocNETBuilder pipelineBuilder = new DocNETBuilder(builder.Services);

            pipelineBuilder.WithFilter<PageToImageFilter>();
            pipelineBuilder.WithFilter<SubPagesFilter>();

            options(pipelineBuilder);

            builder.AddPipeline<DocNETPipeline>(pipelineBuilder.MimeTypes);

            return builder;
        }
    }
}
