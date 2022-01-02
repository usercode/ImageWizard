using ImageWizard.Caches;
using ImageWizard.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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
            return endpoints.MapMethods($"{path}/{{*path}}", new[] { "GET", "HEAD" }, new ImageWizardApi().ExecuteAsync);
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

            services.AddSingleton(configuration);

            //services.AddSingleton<IStreamPool, MemoryStreamPool>();
            services.AddSingleton<IStreamPool, RecyclableMemoryStreamPool>();

            services.AddTransient<ICache, OneTimeCache>();

            return configuration;
        }
    }
}
