using ImageWizard.SvgNet.Builder;
using ImageWizard.SvgNet.Filters;
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
        public static IImageWizardBuilder AddSvgNet(this IImageWizardBuilder builder)
        {
            return AddSvgNet(builder, x => x.WithMimeTypes(MimeTypes.Svg));
        }

        public static IImageWizardBuilder AddSvgNet(this IImageWizardBuilder builder, Action<ISvgNetBuilder> options)
        {
            SvgNetBuilder pipelineBuilder = new SvgNetBuilder(builder.Services);

            pipelineBuilder.WithFilter<RemoveSizeFilter>();
            pipelineBuilder.WithFilter<RotateFilter>();
            pipelineBuilder.WithFilter<BlurFilter>();
            pipelineBuilder.WithFilter<GrayscaleFilter>();
            pipelineBuilder.WithFilter<InvertFilter>();
            pipelineBuilder.WithFilter<SaturateFilter>();
            pipelineBuilder.WithFilter<ImageFormatFilter>();

            options(pipelineBuilder);

            builder.AddPipeline<SvgPipeline>(pipelineBuilder.MimeTypes);

            return builder;
        }
    }
}
