using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ImageWizard.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ImageWizard.Analytics;
using ImageWizard.Azure;
using System.Web;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using ImageWizard.Core.ImageLoaders;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;

namespace ImageWizard.TestApp
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
            //generate random key
            byte[] keyBuffer = new byte[64];
            RandomNumberGenerator.Create().GetBytes(keyBuffer);

            string key = WebEncoders.Base64UrlEncode(keyBuffer);
            //string key = "DEMO-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---==";

            services.AddImageWizard(x =>
            {
                x.AllowUnsafeUrl = false;
                x.UseClintHints = true;
                x.UseETag = true;
                x.Key = key;
            })
                //.SetFileCache()
                //.SetMongoDBCache()
                .AddHttpLoader(x=>
                {
                    //x.RefreshMode = ImageLoaderRefreshMode.EveryTime;
                    x.SetHeader("Api", "XYZ");
                })
                .AddFileLoader()
                .AddYoutubeLoader()
                .AddGravatarLoader()
                .AddAnalytics()
                .AddAzureBlob(x =>
                {
                    x.ConnectionString = "";
                    x.ContainerName = "MyContainer";
                })
                ;

            services.AddImageWizardClient(x =>
            {
                x.UseUnsafeUrl = false;
                x.Key = key;
            });

            services.AddRazorPages();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
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
