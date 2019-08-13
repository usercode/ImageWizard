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
        public static IServiceCollection AddImageWizardClient(this IServiceCollection services)
        {
            return AddImageWizardClient(services, x => { });
        }

        public static IServiceCollection AddImageWizardClient(this IServiceCollection services, Action<ImageWizardClientSettings> setup)
        {
            services.Configure(setup);

            services.AddTransient<ImageUrlBuilder>();

            services.AddHttpContextAccessor();

            return services;
        }

        public static IImageLoaderType ImageWizard(this IUrlHelper htmlHelper)
        {
            ImageUrlBuilder imageWizard = htmlHelper.ActionContext.HttpContext.RequestServices.GetRequiredService<ImageUrlBuilder>();

            return imageWizard;
        }
    }
}
