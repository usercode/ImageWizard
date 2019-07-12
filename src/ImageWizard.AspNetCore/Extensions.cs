using ImageWizard.AspNetCore.Builder;
using ImageWizard.AspNetCore.Builder.Types;
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
        public static IServiceCollection AddImageWizard(this IServiceCollection services)
        {
            return AddImageWizard(services, x => { });
        }

        public static IServiceCollection AddImageWizard(this IServiceCollection services, Action<ImageWizardSettings> setup)
        {
            services.Configure(setup);

            services.AddTransient<ImageUrlBuilder>();

            return services;
        }

        public static IImageSelector ImageWizard(this IUrlHelper htmlHelper)
        {
            ImageUrlBuilder imageWizard = htmlHelper.ActionContext.HttpContext.RequestServices.GetRequiredService<ImageUrlBuilder>();

            return imageWizard;
        }
    }
}
