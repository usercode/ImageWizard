using ImageWizard.Core.ImageFilters.Options;
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
            builder.AddFilter<ResizeFilter>();
            builder.AddFilter<CropFilter>();
            builder.AddFilter<GrayscaleFilter>();
            builder.AddFilter<BlackWhiteFilter>();
            builder.AddFilter<TrimFilter>();
            builder.AddFilter<FlipFilter>();
            builder.AddFilter<RotateFilter>();
            builder.AddFilter<BlurFilter>();
            builder.AddFilter<TextFilter>();
            builder.AddFilter<InvertFilter>();
            builder.AddFilter<BrightnessFilter>();
            builder.AddFilter<ContrastFilter>();
            builder.AddFilter<DPRFilter>();
            builder.AddFilter<NoCacheFilter>();
            builder.AddFilter<AutoOrientFilter>();

            //formats
            builder.AddFilter<JpgFilter>();
            builder.AddFilter<PngFilter>();
            builder.AddFilter<GifFilter>();
            builder.AddFilter<BmpFilter>();

            return builder;
        }
    }
}
