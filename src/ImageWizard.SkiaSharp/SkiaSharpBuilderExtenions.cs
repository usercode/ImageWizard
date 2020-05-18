using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp
{
    public static class SkiaSharpBuilderExtenions
    {
        public static IImageWizardBuilder AddSkiaSharp(this IImageWizardBuilder builder)
        {
            return AddSkiaSharp(builder, MimeTypes.WebP, MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bitmap);
        }

        public static IImageWizardBuilder AddSkiaSharp(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            builder.AddPipeline<SkiaSharpPipeline>(mimeTypes);

            builder.Services.AddSingleton<IProcessingPipeline, SkiaSharpPipeline>();

            return builder;
        }
    }
}
