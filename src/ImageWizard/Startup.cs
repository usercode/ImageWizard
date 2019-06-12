using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ImageWizard.Filters;
using ImageWizard.Services;
using ImageWizard.Settings;
using ImageWizard.Filters.ImageFormats;
using Microsoft.AspNetCore.Http;
using ImageWizard.ImageLoaders;
using ImageWizard.ImageStorages;
using ImageWizard.SharedContract;

namespace ImageWizard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ServiceSettings>(Configuration.GetSection("ImageWizard"));

            FilterManager filterManager = new FilterManager();
            filterManager.Register<ResizeFilter>();
            filterManager.Register<CropFilter>();
            filterManager.Register<GrayscaleFilter>();
            filterManager.Register<BlackWhiteFilter>();
            filterManager.Register<TrimFilter>();
            filterManager.Register<FlipFilter>();
            filterManager.Register<RotateFilter>();
            filterManager.Register<BlurFilter>();
            filterManager.Register<TextFilter>();

            //formats
            filterManager.Register<JpgFilter>();
            filterManager.Register<PngFilter>();
            filterManager.Register<GifFilter>();
            filterManager.Register<BmpFilter>();

            services.AddSingleton(filterManager);

            services.AddHttpClient<HttpLoader>();

            services.AddSingleton(x => new CryptoService(x.GetRequiredService<IOptions<ServiceSettings>>().Value.Key));
            services.AddSingleton<FileStorage>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpsRedirection(x => x.RedirectStatusCode = StatusCodes.Status308PermanentRedirect);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
