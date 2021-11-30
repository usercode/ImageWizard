using ImageWizard.Client.Builder.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ImageWizard.Client
{
    /// <summary>
    /// Extensions
    /// </summary>
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

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            return services;
        }

        public static IImageLoader ImageWizard(this IUrlHelper urlHelper)
        {
            ImageUrlBuilder imageWizard = urlHelper.ActionContext.HttpContext.RequestServices.GetRequiredService<ImageUrlBuilder>();

            return imageWizard;
        }
    }
}
