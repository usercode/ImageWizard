using ImageWizard.AspNetCore.Builder;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore
{
    public static class Extensions
    {
        public static IServiceCollection AddImageWizard(this IServiceCollection  services)
        {
            //services.AddSingleton<CryptoService>();
            //services.AddTransient<ImageUrlBuilder>();

            return services;
        }

        public static ImageUrlBuilder ImageWizard(this IUrlHelper htmlHelper, string imageUrl)
        {
            var settings = htmlHelper.ActionContext.HttpContext.RequestServices.GetRequiredService<IOptions<ImageWizardSettings>>();

            return new ImageUrlBuilder(imageUrl, settings.Value);
        }
    }
}
