using ImageWizard.Core.Middlewares;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class FilterExtensions
    {
        /// <summary>
        /// RegisterFilter
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        public static IImageWizardBuilder AddFilter<TFilter>(this IImageWizardBuilder builder)
           where TFilter : class, IFilter, new()
        {
            builder.FilterManager.Register<TFilter>();

            return builder;
        }

        /// <summary>
        /// AddDefaultFilters
        /// </summary>
        public static IImageWizardBuilder AddDefaultFilters(this IImageWizardBuilder builder)
        {
            builder.FilterManager.Register<ResizeFilter>();
            builder.FilterManager.Register<CropFilter>();
            builder.FilterManager.Register<GrayscaleFilter>();
            builder.FilterManager.Register<BlackWhiteFilter>();
            builder.FilterManager.Register<TrimFilter>();
            builder.FilterManager.Register<FlipFilter>();
            builder.FilterManager.Register<RotateFilter>();
            builder.FilterManager.Register<BlurFilter>();
            builder.FilterManager.Register<TextFilter>();

            //formats
            builder.FilterManager.Register<JpgFilter>();
            builder.FilterManager.Register<PngFilter>();
            builder.FilterManager.Register<GifFilter>();
            builder.FilterManager.Register<BmpFilter>();

            return builder;
        }
    }
}
