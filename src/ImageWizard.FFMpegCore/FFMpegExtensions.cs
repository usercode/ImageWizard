using ImageWizard.FFMpegCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.FFMpegCore
{
    public static class FFMpegExtensions
    {
        public static IImageSharpBuilder AddFFMpegCore(this IImageWizardBuilder builder)
        {
            return AddFFMpegCore(builder, MimeTypes.Mpeg, MimeTypes.Mp4, MimeTypes.Mobile3GP, MimeTypes.Avi);
        }

        public static IImageSharpBuilder AddFFMpegCore(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            builder.AddPipeline<FFMpegPipeline>(mimeTypes);

            return new FFMpegBuilder(builder);
        }
    }
}
