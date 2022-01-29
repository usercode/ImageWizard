using ImageWizard.ImageSharp;
using ImageWizard.ImageSharp.Filters;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    public delegate void ImagePreProcessing(ImageSharpFilterContext context);
    public delegate void ImagePostProcessing(ImageSharpFilterContext context);

    public static class ImageSharpBuilderExtensions
    {
        public static IImageSharpBuilder WithFilter<TFilter>(this IImageSharpBuilder builder) where TFilter : ImageSharpFilter
        {
            builder.Services.AddTransient<TFilter>();
            builder.Services.AddSingleton(new PipelineAction<ImageSharpPipeline>(x => x.AddFilter<TFilter>()));

            return builder;
        }

        public static IImageSharpBuilder WithOptions(this IImageSharpBuilder builder, Action<ImageSharpOptions> action)
        {
            builder.Services.Configure(action);

            return builder;
        }

        public static IImageSharpBuilder WithPreProcessing(this IImageSharpBuilder builder, ImagePreProcessing action)
        {
            builder.Services.AddSingleton(action);

            return builder;
        }

        public static IImageSharpBuilder WithPostProcessing(this IImageSharpBuilder builder, ImagePostProcessing action)
        {
            builder.Services.AddSingleton(action);

            return builder;
        }
    }
}
