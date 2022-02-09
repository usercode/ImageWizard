﻿using ImageWizard.Caches;
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
        /// <summary>
        /// Maps ImageWizard API with default base path. ("/image")
        /// </summary>
        /// <param name="endpoints"></param>
        /// <returns></returns>
        public static IEndpointConventionBuilder MapImageWizard(this IEndpointRouteBuilder endpoints)
        {
            return MapImageWizard(endpoints, ImageWizardDefaults.BasePath);
        }

        /// <summary>
        /// Maps ImageWizard API with specified base path.
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEndpointConventionBuilder MapImageWizard(this IEndpointRouteBuilder endpoints, PathString path)
        {
            return endpoints
                        .MapMethods($"{path}/{{*path}}", new[] { HttpMethods.Get, HttpMethods.Head }, new ImageWizardApi().ExecuteAsync)
                        .WithName("ImageWizard");
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

        /// <summary>
        /// Adds ImageWizard services.
        /// <br /><br />
        /// Default services:<br/>
        /// <see cref="ICache"/> -> <see cref="OneTimeCache"/><br/>
        /// <see cref="ICacheKey"/> -> <see cref="SHA256CacheKey"/><br/>
        /// <see cref="ICacheHash"/> -> <see cref="SHA256CacheHash"/><br/>
        /// <see cref="IUrlSignature"/> -> <see cref="HMACSHA256UrlSignature"/><br/>
        /// <see cref="IStreamPool"/> -> <see cref="RecyclableMemoryStreamPool"/><br/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IImageWizardBuilder AddImageWizard(this IServiceCollection services)
        {
            return AddImageWizard(services, options => { });
        }

        /// <summary>
        /// Adds ImageWizard services with custom options.
        /// <br /><br />
        /// Default services:<br/>
        /// <see cref="ICache"/> -> <see cref="OneTimeCache"/><br/>
        /// <see cref="ICacheKey"/> -> <see cref="SHA256CacheKey"/><br/>
        /// <see cref="ICacheHash"/> -> <see cref="SHA256CacheHash"/><br/>
        /// <see cref="IUrlSignature"/> -> <see cref="HMACSHA256UrlSignature"/><br/>
        /// <see cref="IStreamPool"/> -> <see cref="RecyclableMemoryStreamPool"/><br/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IImageWizardBuilder AddImageWizard(this IServiceCollection services, Action<ImageWizardOptions> options)
        {
            services.Configure(options);

            services.AddHttpContextAccessor();

            //services.AddSingleton<IStreamPool, MemoryStreamPool>();
            services.AddSingleton<IStreamPool, RecyclableMemoryStreamPool>();

            services.AddTransient<ICache, OneTimeCache>();
            services.AddTransient<ICacheKey, SHA256CacheKey>();
            services.AddTransient<ICacheHash, SHA256CacheHash>();
            services.AddTransient<IUrlSignature, HMACSHA256UrlSignature>();

            ImageWizardBuilder configuration = new ImageWizardBuilder(services);
            services.AddSingleton(configuration);

            return configuration;
        }
    }
}
