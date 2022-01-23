using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ImageWizard.DocNET;
using Microsoft.AspNetCore.HttpOverrides;
using ImageWizard.Caches;
using ImageWizard.Loaders;
using ImageWizard.SkiaSharp;
using ImageWizard.Azure;

namespace ImageWizard
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ImageWizardOptions>(Configuration.GetSection("General"));
            services.Configure<HttpLoaderOptions>(Configuration.GetSection("HttpLoader"));
            services.Configure<FileCacheSettings>(Configuration.GetSection("FileCache"));
            services.Configure<FileLoaderOptions>(Configuration.GetSection("FileLoader"));

            services.AddControllersWithViews();
            services.AddRazorPages();

            IImageWizardBuilder imageWizard = services.AddImageWizard()
                                                        .AddImageSharp()
                                                        .AddSkiaSharp(MimeTypes.WebP)
                                                        .AddSvgNet()
                                                        .AddDocNET()
                                                        .AddHttpLoader()
                                                        .AddFileLoader()
                                                        .AddYoutubeLoader()
                                                        .AddGravatarLoader()
                                                        .AddAzureLoader()
                                                        .SetFileCache();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(x =>
            {
                x.MapImageWizard();
                x.MapRazorPages();
                x.MapControllers();
            });
        }
    }
}
