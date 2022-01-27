using ImageWizard.Caches;
using ImageWizard.Core;
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
            return MapImageWizard(endpoints, ImageWizardDefaults.BasePath);
        }

        public static IEndpointConventionBuilder MapImageWizard(this IEndpointRouteBuilder endpoints, PathString path)
        {
            return endpoints.MapMethods($"{path}/{{*path}}", new[] { HttpMethods.Get, HttpMethods.Head }, new ImageWizardApi().ExecuteAsync);
        }

        public static IApplicationBuilder UseImageWizard(this IApplicationBuilder builder)
        {
            return UseImageWizard(builder, ImageWizardDefaults.BasePath);
        }

        public static IApplicationBuilder UseImageWizard(this IApplicationBuilder builder, Action<IImageWizardEndpointBuilder>? endpointsHandler = null)
        {
            return UseImageWizard(builder, ImageWizardDefaults.BasePath, endpointsHandler);
        }

        public static IApplicationBuilder UseImageWizard(this IApplicationBuilder builder, PathString path, Action<IImageWizardEndpointBuilder>? endpointsHandler = null)
        {
            builder.Map(path, x =>
            {
                x.UseRouting();
                x.UseEndpoints(endpoints =>
                {
                    endpoints.MapImageWizard(PathString.Empty);

                    endpointsHandler?.Invoke(new ImageWizardEndpointBuilder(endpoints));
                });
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

            //services.AddSingleton<IStreamPool, MemoryStreamPool>();
            services.AddSingleton<IStreamPool, RecyclableMemoryStreamPool>();

            services.AddTransient<ICache, OneTimeCache>();
            services.AddHttpContextAccessor();

            services.AddSingleton<ICacheKey, SHA256CacheKey>();

            ImageWizardBuilder configuration = new ImageWizardBuilder(services);
            services.AddSingleton(configuration);

            return configuration;
        }
    }
}
