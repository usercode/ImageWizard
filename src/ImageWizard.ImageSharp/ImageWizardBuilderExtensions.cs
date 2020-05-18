using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.ImageSharp;
using ImageWizard.ImageSharp.Builder;
using ImageWizard.ImageSharp.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        public static IImageSharpBuilder AddImageSharp(this IImageWizardBuilder builder)
        {
            return AddImageSharp(builder, MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bitmap);
        }

        public static IImageSharpBuilder AddImageSharp(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            builder.AddPipeline<ImageSharpPipeline>(mimeTypes);

            builder.Services.AddSingleton<IProcessingPipeline, ImageSharpPipeline>();

            return new ImageSharpBuilder(builder);
        }
    }
}
