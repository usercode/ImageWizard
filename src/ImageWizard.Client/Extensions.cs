using ImageWizard.Client.Builder.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ImageWizard
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
            imageWizard.UrlHelper = htmlHelper;

            return imageWizard;
        }
    }
}
