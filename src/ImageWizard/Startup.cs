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
using ImageWizard.Analytics;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;

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

        public bool UseAnalytics = false;

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ImageWizardOptions>(Configuration.GetSection("General"));
            services.Configure<HttpLoaderOptions>(Configuration.GetSection("HttpLoader"));
            services.Configure<FileCacheSettings>(Configuration.GetSection("FileCache"));
            services.Configure<FileLoaderOptions>(Configuration.GetSection("FileLoader"));
            services.Configure<AzureBlobOptions>(Configuration.GetSection("Azure"));
            services.Configure<ImageWizardAppOptions>(Configuration.GetSection("App"));

            IImageWizardBuilder imageWizard = services.AddImageWizard()
                                                        .AddImageSharp(i => i
                                                            .WithPreProcessing(x =>
                                                            {
                                                                x.Image.Mutate(m => m.AutoOrient());
                                                            })
                                                            .WithPostProcessing(x =>
                                                            {
                                                                IOptions<ImageWizardAppOptions> options = x.ProcessingContext
                                                                                                           .ServiceProvider.GetRequiredService<IOptions<ImageWizardAppOptions>>();

                                                                if (options.Value.AddMetadata)
                                                                {
                                                                    x.Image.Metadata.ExifProfile = new ExifProfile();
                                                                    x.Image.Metadata.ExifProfile.SetValue(ExifTag.Copyright, options.Value.MetadataCopyright);
                                                                }
                                                            }))
                                                        .AddSkiaSharp(MimeTypes.WebP)
                                                        .AddSvgNet()
                                                        .AddDocNET()
                                                        .AddHttpLoader()
                                                        .AddFileLoader()
                                                        .AddYoutubeLoader()
                                                        .AddGravatarLoader()
                                                        .AddAzureLoader()
                                                        .AddOpenGraphLoader()
                                                        .AddAnalytics()
                                                        .SetFileCache()
                                                        ;

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ImageWizardAppOptions> options)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            app.UseImageWizard(x =>
            {
                if (options.Value.UseAnalytics)
                {
                    x.MapAnalytics();
                }
            });
        }
    }
}
