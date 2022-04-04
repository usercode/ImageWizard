// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using ImageWizard.ImageSharp.Filters;
using Microsoft.AspNetCore.HttpOverrides;
using ImageWizard.Client;
using ImageWizard.OpenCvSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using ImageWizard.Loaders;
using ImageWizard.Cleanup;
using System;
using ImageWizard.MongoDB;

namespace ImageWizard.TestApp;

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
            x.UseClientHints = false;
            x.UseETag = true;
            x.Key = key;
            x.RefreshLastAccessInterval = TimeSpan.FromMinutes(1);
        })
            .AddImageSharp(c => c
                .WithMimeTypes(MimeTypes.WebP, MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bmp)
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
                    if (x.ImageFormat is ImageSharp.JpegFormat or ImageSharp.PngFormat)
                    {
                        x.ImageFormat = new ImageSharp.WebPFormat();
                    }

                    //override metadata
                    x.Image.Metadata.ExifProfile = new ExifProfile();
                    x.Image.Metadata.ExifProfile.SetValue(ExifTag.Copyright, "ImageWizard");
                }))
            //.AddSkiaSharp(c => c                    
            //    .WithOptions(x =>
            //                {
            //                    x.ImageMaxHeight = 4000;
            //                    x.ImageMaxWidth = 4000;
            //                })
            //    .WithFilter<ImageWizard.SkiaSharp.Filters.ResizeFilter>())
            .AddSvgNet()
            .AddHttpLoader(x =>
            {
                x.RefreshMode = LoaderRefreshMode.None;

                x.SetHeader("Api", "XYZ");

                x.AllowedHosts = new[] { "upload.wikimedia.org" };
                x.AllowAbsoluteUrls = true;
            })
            .AddFileLoader()
            .AddYoutubeLoader(x => x.RefreshMode = LoaderRefreshMode.None)
            .AddGravatarLoader(x => x.RefreshMode = LoaderRefreshMode.None)
            .AddPuppeteerLoader(x => x.RefreshMode = LoaderRefreshMode.None)
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
            .SetMongoDBCache()
            //.SetDistributedCache()
            //.SetFileCacheV1()
            //.SetFileCacheV2()
            .AddCleanupService(x =>
                                    {
                                        x.Interval = TimeSpan.FromMinutes(1);

                                        x.OlderThan(TimeSpan.FromMinutes(2));
                                        x.LastUsedSince(TimeSpan.FromMinutes(2));
                                    })
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
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddHttpsRedirection(x => x.HttpsPort = 443);
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
        app.UseImageWizard(x => x.MapAnalytics());
        app.UseRouting();
        app.UseEndpoints(x =>
        {
            x.MapRazorPages();
        });
    }
}
