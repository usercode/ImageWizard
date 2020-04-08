using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using ImageWizard.MongoDB;
using ImageWizard.Core.ImageCaches;
using ImageWizard.Settings;
using ImageWizard.MongoDB.ImageCaches;
using System.Collections.Generic;
using ImageWizard.ImageStorages;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.ImageLoaders.Files;
using Microsoft.Extensions.Hosting;
using ImageWizard.Analytics;

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
            services.Configure<MongoDBCacheOptions>(Configuration.GetSection("MongoDBCache"));

            services.AddControllersWithViews();
            services.AddRazorPages();

            string cache = Configuration.GetSection("General")["Cache"];

            IImageWizardBuilder imageWizard = services.AddImageWizard()
                                                        .AddHttpLoader()
                                                        .AddFileLoader()
                                                        .AddYoutubeLoader()
                                                        .AddGravatarLoader()
                                                        .AddAnalytics();

            switch (cache)
            {
                case "InMemory":
                    imageWizard.SetDistributedCache();
                    break;

                case "File":
                    imageWizard.SetFileCache();
                    break;

                case "MongoDB":
                    imageWizard.SetMongoDBCache();
                    break;

                default:
                    throw new Exception("unknown cache type selected");
            }

            services.AddHttpsRedirection(x => { x.RedirectStatusCode = StatusCodes.Status301MovedPermanently; x.HttpsPort = 443; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(x =>
            {
                x.MapRazorPages();
                x.MapControllers();
                x.MapImageWizard();
            });
        }
    }
}
