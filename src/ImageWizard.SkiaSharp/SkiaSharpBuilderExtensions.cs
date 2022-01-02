using ImageWizard.SkiaSharp.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp
{
    public static class SkiaSharpBuilderExtensions
    {
        public static ISkiaSharpBuilder AddSkiaSharp(this IImageWizardBuilder builder)
        {
            return AddSkiaSharp(builder, MimeTypes.WebP, MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bmp);
        }

        public static ISkiaSharpBuilder AddSkiaSharp(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            builder.AddPipeline<SkiaSharpPipeline>(mimeTypes);

            return new SkiaSharpBuilder(builder);
        }
    }
}
