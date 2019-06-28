using ImageWizard.Core.ImageCaches;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ImageWizardSettings serviceSettings = new ImageWizardSettings();
            Configuration.GetSection("ImageWizard").Bind(serviceSettings);

            services.AddImageWizard(new ImageWizardCoreSettings() { AllowUnsafeUrl = serviceSettings.AllowUnsafeUrl, Key = serviceSettings.Key })
                        .AddDefaultFilters()
                        .AddFileCache(new FileCacheSettings(HostingEnvironment.WebRootPath))
                        .AddHttpLoader(new HttpLoaderSettings())
                       ;

            services.AddHttpsRedirection(x => x.RedirectStatusCode = StatusCodes.Status308PermanentRedirect);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseImageWizard();
        }
    }
}
