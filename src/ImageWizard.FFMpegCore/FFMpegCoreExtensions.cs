using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.FFMpegCore
{
    public static class FFMpegCoreExtensions
    {
        public static IImageWizardBuilder AddFFMpegCore(this IImageWizardBuilder builder)
        {
            return AddFFMpegCore(builder, MimeTypes.Mpeg, MimeTypes.Mp4, MimeTypes.Mobile3GP, MimeTypes.Avi);
        }

        public static IImageWizardBuilder AddFFMpegCore(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            builder.AddPipeline<FFMpegCorePipeline>(mimeTypes);

            return builder;
        }
    }
}
