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
        public static IImageWizardBuilder AddImageSharp(this IImageWizardBuilder builder)
        {
            return AddImageSharp(builder, x => { }, null);
        }

        public static IImageWizardBuilder AddImageSharp(this IImageWizardBuilder builder, Action<ImageSharpOptions> options, PipelineAction<IImageSharpBuilder> action = null)
        {
            builder.Services.Configure(options);

            builder.AddPipeline<ImageSharpPipeline>(new[] { MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bitmap });

            if(action == null)
            {
                action = x => { };
            }

            builder.Services.AddSingleton<IProcessingPipeline, ImageSharpPipeline>();
            builder.Services.AddSingleton(action);

            return builder;
        }
    }
}
