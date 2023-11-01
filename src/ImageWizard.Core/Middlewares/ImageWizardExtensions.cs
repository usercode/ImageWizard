// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Caches;
using ImageWizard.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ImageWizard;

public static class ImageWizardExtensions
{
    /// <summary>
    /// Maps ImageWizard endpoint with specified base path.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapImageWizard(this IEndpointRouteBuilder endpoints, string path = ImageWizardDefaults.BasePath)
    {
        return endpoints
                    .MapMethods($"{path}/{{signature}}/{{*path}}", new[] { HttpMethods.Get, HttpMethods.Head }, ImageWizardApi.ExecuteAsync)
                    .WithDisplayName("ImageWizard");
    }

    /// <summary>
    /// Use ImageWizard middleware with default base path. ("/image")
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="endpointsHandler"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseImageWizard(this IApplicationBuilder builder, Action<IImageWizardEndpointBuilder>? endpointsHandler = null)
    {
        return UseImageWizard(builder, ImageWizardDefaults.BasePath, endpointsHandler);
    }

    /// <summary>
    /// Use ImageWizard middleware with specified base path.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="path"></param>
    /// <param name="endpointsHandler"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseImageWizard(this IApplicationBuilder builder, string path, Action<IImageWizardEndpointBuilder>? endpointsHandler = null)
    {
        builder.Map(path, x =>
        {
            x.UseRouting();
            x.UseEndpoints(endpoints =>
            {
                endpointsHandler?.Invoke(new ImageWizardEndpointBuilder(endpoints));

                endpoints.MapImageWizard(string.Empty);
            });
        });

        return builder;
    }

    /// <summary>
    /// Adds ImageWizard services with custom options.
    /// <br /><br />
    /// Default services:<br/>
    /// <see cref="ICache"/> -> <see cref="OneTimeCache"/><br/>
    /// <see cref="ICacheKey"/> -> <see cref="SHA256CacheKey"/><br/>
    /// <see cref="ICacheHash"/> -> <see cref="SHA256CacheHash"/><br/>
    /// <see cref="ICacheLock"/> -> <see cref="LocalCacheLock"/><br/>
    /// <see cref="IUrlSignature"/> -> <see cref="HMACSHA256UrlSignature"/><br/>
    /// <see cref="IStreamPool"/> -> <see cref="RecyclableMemoryStreamPool"/><br/>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IImageWizardBuilder AddImageWizard(this IServiceCollection services, Action<ImageWizardOptions>? options = null)
    {
        if (options != null)
        {
            services.Configure(options);
        }

        services.AddHttpContextAccessor();

        //services.AddSingleton<IStreamPool, MemoryStreamPool>();
        services.AddSingleton<IStreamPool, RecyclableMemoryStreamPool>();
        services.AddSingleton<ICacheLock, LocalCacheLock>();

        services.AddTransient<ICache, OneTimeCache>();
        services.AddTransient<ICacheKey, SHA256CacheKey>();
        services.AddTransient<ICacheHash, SHA256CacheHash>();

        services.AddSingleton<IUrlSignature, HMACSHA256UrlSignature>();

        services.AddTransient<InterceptorInvoker>();

        ImageWizardBuilder configuration = new ImageWizardBuilder(services);
        services.AddSingleton(configuration);

        return configuration;
    }
}
