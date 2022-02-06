using ImageWizard.Core.Processing.Builder;
using ImageWizard.ImageSharp.Filters;
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
        private readonly static string[] SupportedMimeTypes = new[] { MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Tga, MimeTypes.Bmp };

        /// <summary>
        /// Adds the ImageSharp pipeline with default mime types (jpg, gif, png, tga, bmp).
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IImageWizardBuilder AddImageSharp(this IImageWizardBuilder builder)
        {
            return AddImageSharp(builder, SupportedMimeTypes);
        }

        /// <summary>
        /// Adds the ImageSharp pipeline for defined mime types.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="mimeTypes"></param>
        /// <returns></returns>
        public static IImageWizardBuilder AddImageSharp(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            return AddImageSharp(builder, x => x.WithMimeTypes(mimeTypes));
        }

        /// <summary>
        /// Adds the ImageSharp pipeline with custom actions.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IImageWizardBuilder AddImageSharp(this IImageWizardBuilder builder, Action<IImageSharpBuilder> options)
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

            pipelineBuilder.WithMimeTypes(SupportedMimeTypes);

            options(pipelineBuilder);

            builder.AddPipeline<ImageSharpPipeline>(pipelineBuilder.MimeTypes);

            return builder;
        }
    }
}
