using ImageWizard.AspNetCore.Builder;
using ImageWizard.AspNetCore.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore
{
    public static class Extensions
    {
        public static IServiceCollection AddImageWizard(this IServiceCollection  services)
        {
            services.AddSingleton<CryptoService>();
            services.AddTransient<ImageUrlBuilder>();

            return services;
        }

        public static ImageUrlBuilder ImageWizard(this IHtmlHelper htmlHelper, string imageUrl)
        {
            ImageUrlBuilder builder = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<ImageUrlBuilder>();
            builder.ImageUrl = imageUrl;

            return builder;
        }
    }
}
