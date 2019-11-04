using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.Middlewares;
using ImageWizard.Settings;
using ImageWizard.SharedContract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageWizard
{
    public static class ImageWizardExtensions
    {

        public static IApplicationBuilder UseImageWizard(this IApplicationBuilder app)
        {
            return UseImageWizard(app, "/image");
        }

        public static IApplicationBuilder UseImageWizard(this IApplicationBuilder app, string path)
        {
            app.UseEndpoints(endpoints =>
            {
                RequestDelegate pipeline = endpoints.CreateApplicationBuilder()
                                                        .UseMiddleware<ImageWizardMiddleware>()
                                                        .Build();

                endpoints.Map($"{path}/{{*imagePath}}", pipeline).WithDisplayName("ImageWizard");
            });
            
            return app;
        }

        public static IImageWizardBuilder AddImageWizard(this IServiceCollection services)
        {
            return AddImageWizard(services, options => { });
        }

        public static IImageWizardBuilder AddImageWizard(this IServiceCollection services, Action<ImageWizardSettings> settingsSetup)
        {
            services.Configure(settingsSetup);

            ImageWizardBuilder configuration = new ImageWizardBuilder(services);
            configuration.AddDefaultFilters();
            configuration.AddHttpLoader();
            configuration.SetDistributedCache();

            return configuration;
        }   
    }
}
