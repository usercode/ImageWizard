// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using ImageWizard.ImageSharp.Filters;
using Microsoft.AspNetCore.HttpOverrides;
using ImageWizard.Client;
using ImageWizard.OpenCvSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using ImageWizard.Loaders;
using System;
using ImageWizard.MongoDB;
using ImageWizard.ImageSharp;
using ImageWizard.Caches;

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
        services.Configure<ImageWizardOptions>(Configuration.GetSection("General"));
        services.Configure<HttpLoaderOptions>(Configuration.GetSection("HttpLoader"));
        services.Configure<FileCacheOptions>(Configuration.GetSection("FileCache"));
        services.Configure<FileLoaderOptions>(Configuration.GetSection("FileLoader"));
        services.Configure<WatermarkOptions>(Configuration.GetSection("Watermark"));

        //generate random key
        byte[] key = RandomNumberGenerator.GetBytes(64);

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
            x.WhenLoaderFailedUseExistingCachedData();

            //x.FallbackHandler = (state, url, cachedData) =>
            //{
            //    //use the existing cached data if available?
            //    if (cachedData != null)
            //    {
            //        return cachedData;
            //    }

            //    //load fallback image
            //    FileInfo fallbackImage = state switch
            //    {
            //        LoaderResultState.NotFound => new FileInfo(@"C:\notfound.jpg"),
            //        LoaderResultState.Failed => new FileInfo(@"C:\failed.jpg"),
            //       _ => throw new Exception()
            //    };

            //    if (fallbackImage.Exists == false)
            //    {
            //        return null;
            //    }

            //    return fallbackImage.ToCachedData();
            //};
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
                    if (x.ImageFormat is JpegFormat)
                    {
                        x.ImageFormat = new WebPFormat() { Lossless = false };
                    }
                    else if (x.ImageFormat is PngFormat)
                    {
                        x.ImageFormat = new WebPFormat() { Lossless = true };
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
            //.AddPlaywrightLoader(x => x.RefreshMode = LoaderRefreshMode.None)
            .AddOpenGraphLoader(x => x.RefreshMode = LoaderRefreshMode.None)
            .AddFFMpegCore()
            .AddDocNET()
            .AddOpenCvSharp()
            .AddOpenStreetMapLoader()
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
            .SetFileCache()

            //Adds a background service which removes cached data based on defined CleanupReason.
            //The cache needs to implements <see cref="ICleanupCache"/>.
            //.AddCleanupService(x =>
            //                        {
            //                            //Duration between the cleanup actions. (Default: 1 day)
            //                            x.Interval = TimeSpan.FromMinutes(1);

            //                            //Removes cached data which are older than defined duration. (see IMetadata.Created)
            //                            x.OlderThan(TimeSpan.FromMinutes(2));

            //                            //Removes cached data which are last used since defined duration. (see IMetadata.LastAccess)
            //                            x.LastUsedSince(TimeSpan.FromMinutes(2));

            //                            //Removes cached data which are expired (based on the loader result).
            //                            x.Expired();
            //                        })
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
        app.UseImageWizard(x =>
        {
            x.MapAnalytics();
            x.MapOpenStreetMap();
        }
        );
        app.UseRouting();
        app.UseEndpoints(x =>
        {
            x.MapRazorPages();
        });
    }
}
