using ImageWizard.FFMpegCore;
using ImageWizard.FFMpegCore.Builder;
using ImageWizard.FFMpegCore.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class FFMpegExtensions
    {
        public static IImageWizardBuilder AddFFMpegCore(this IImageWizardBuilder builder)
        {
            return AddFFMpegCore(builder, MimeTypes.Mpeg, MimeTypes.Mp4, MimeTypes.Mobile3GP, MimeTypes.Avi);
        }

        public static IImageWizardBuilder AddFFMpegCore(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            return AddFFMpegCore(builder, x => x.WithMimeTypes(mimeTypes));
        }

        public static IImageWizardBuilder AddFFMpegCore(this IImageWizardBuilder builder, Action<IFFMpegBuilder> options)
        {
            FFMpegBuilder pipelineBuilder = new FFMpegBuilder(builder.Services);

            pipelineBuilder.WithFilter<FrameFilter>();

            options(pipelineBuilder);

            builder.AddPipeline<FFMpegPipeline>(pipelineBuilder.MimeTypes);

            return builder;
        }
    }
}
