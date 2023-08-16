// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.Client;

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

        services.AddHttpContextAccessor();
        services.AddTransient<IImageWizardUrlBuilder, UrlBuilder>();
        
        services.AddSingleton<IUrlSignature, HMACSHA256UrlSignature>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddScoped<IUrlHelper>(x =>
        {
            var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            var factory = x.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContext);
        });

        return services;
    }

    public static IImageWizardUrlBuilder ImageWizard(this IUrlHelper urlHelper)
    {
        IImageWizardUrlBuilder imageWizard = urlHelper.ActionContext.HttpContext.RequestServices.GetRequiredService<IImageWizardUrlBuilder>();

        return imageWizard;
    }
}
