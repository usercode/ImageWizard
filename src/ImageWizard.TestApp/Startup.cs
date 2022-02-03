using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ImageWizard.Analytics;
using ImageWizard.Azure;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using ImageWizard.ImageSharp.Filters;
using ImageWizard.AWS;
using ImageWizard.SkiaSharp;
using Microsoft.Extensions.Options;
using ImageWizard.FFMpegCore;
using System.Text;
using ImageWizard.DocNET;
using Microsoft.AspNetCore.HttpOverrides;
using ImageWizard.Client;
using ImageWizard.OpenCvSharp;
using ImageWizard.Caches;
using SixLabors.ImageSharp.Processing;
using ImageWizard.Core.Processing.Builder;
using ImageWizard.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

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
            byte[] keyBuffer = RandomNumberGenerator.GetBytes(64);

            string key = WebEncoders.Base64UrlEncode(keyBuffer);

            services.AddImageWizard(x =>
            {
#if DEBUG
                x.AllowUnsafeUrl = true;
#endif
                x.UseAcceptHeader = true;
                x.UseClintHints = false;
                x.UseETag = true;
                x.Key = key;
            })
                .AddImageSharp(c => c
                    .WithMimeTypes(MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bmp)
                    .WithOptions(x =>
                                {
                                    x.ImageMaxHeight = 4000;
                                    x.ImageMaxWidth = 4000;
                                })
                    .WithFilter<ResizeFilter>()
                    .WithPreProcessing(x =>
                    {
                        x.Image.Mutate(m => m.AutoOrient());
                    })
                    .WithPostProcessing(x =>
                    {
                        //override target format
                        //x.ImageFormat = new JpegFormat();

                        //override metadata
                        x.Image.Metadata.ExifProfile = new ExifProfile();
                        x.Image.Metadata.ExifProfile.SetValue(ExifTag.Copyright, "ImageWizard");
                    }))
                .AddSkiaSharp(c => c
                    .WithMimeTypes(MimeTypes.WebP)
                    .WithOptions(x =>
                                {
                                    x.ImageMaxHeight = 4000;
                                    x.ImageMaxWidth = 4000;
                                })
                    .WithFilter<ImageWizard.SkiaSharp.Filters.ResizeFilter>())
                .AddSvgNet()
                //.SetFileCache()
                //.SetMongoDBCache()
                .AddHttpLoader(x =>
                {
                    //x.RefreshMode = ImageLoaderRefreshMode.EveryTime;
                    x.SetHeader("Api", "XYZ");

                    x.AllowedHosts = new [] { "upload.wikimedia.org" };
                    x.AllowAbsoluteUrls = true;
                })
                .AddFileLoader()
                .AddYoutubeLoader()
                .AddGravatarLoader()
                .AddFFMpegCore()
                .AddDocNET()
                .AddOpenCvSharp()
                .AddOpenGraphLoader()
                .AddAnalytics()
                .AddAzureLoader(x =>
                {
                    x.ConnectionString = "";
                    x.ContainerName = "MyContainer";
                })
                .AddAWSLoader(x =>
                {
                    x.AccessKeyId = "";
                    x.SecretAccessKey = "";
                    x.BucketName = "MyBucket";
                })
                //.SetDistributedCache()
                .SetFileCache()
                ;

            services.AddImageWizardClient(x =>
            {
#if DEBUG
                x.UseUnsafeUrl = false;
#endif
                x.Key = key;
            });

            services.AddRazorPages();

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
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseImageWizard(x => x.UseAnalytics());
            app.UseRouting();
            app.UseEndpoints(x =>
            {
                x.MapRazorPages();
            });
        }
    }
}
