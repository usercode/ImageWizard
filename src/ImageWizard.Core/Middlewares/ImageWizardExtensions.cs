using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.ImageLoaders;
using ImageWizard.ImageStorages;
using ImageWizard.Middlewares;
using ImageWizard.Settings;
using ImageWizard.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageWizard
{
    public static class ImageWizardExtensions
    {

        public static IEndpointConventionBuilder MapImageWizard(this IEndpointRouteBuilder endpoints)
        {
            return MapImageWizard(endpoints, ImageWizardConstants.DefaultBasePath);
        }

        public static IEndpointConventionBuilder MapImageWizard(this IEndpointRouteBuilder endpoints, PathString path)
        {
            RequestDelegate pipeline = endpoints.CreateApplicationBuilder()
                                                         .UseMiddleware<ImageWizardMiddleware>()
                                                         .Build();

            return endpoints.MapMethods($"{path}/{{*imagePath}}", new[] { "GET", "HEAD" }, pipeline).WithDisplayName("ImageWizard");
        }

        public static IApplicationBuilder UseImageWizard(this IApplicationBuilder builder)
        {
            return UseImageWizard(builder, ImageWizardConstants.DefaultBasePath);
        }

        public static IApplicationBuilder UseImageWizard(this IApplicationBuilder builder, PathString path)
        {
            builder.Map(path, x =>
            {
                x.UseRouting();
                x.UseEndpoints(endpoits => endpoits.MapImageWizard(PathString.Empty));

            });

            return builder;
        }

        public static IImageWizardBuilder AddImageWizard(this IServiceCollection services)
        {
            return AddImageWizard(services, options => { });
        }

        public static IImageWizardBuilder AddImageWizard(this IServiceCollection services, Action<ImageWizardOptions> options)
        {
            services.Configure(options);

            ImageWizardBuilder configuration = new ImageWizardBuilder(services);
            configuration.AddHttpLoader();
            configuration.SetDistributedCache();

            services.AddSingleton(configuration);

            return configuration;
        }
    }
}
