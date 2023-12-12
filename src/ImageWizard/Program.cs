// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;
using ImageWizard.ImageSharp;
using JpegFormat = ImageWizard.ImageSharp.JpegFormat;
using PngFormat = ImageWizard.ImageSharp.PngFormat;
using ImageWizard.ImageSharp.Filters;
using ImageWizard;
using ImageWizard.Loaders;
using ImageWizard.Caches;
using ImageWizard.Azure;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ImageWizardAppOptions>().BindConfiguration("App");
builder.Services.AddOptions<ImageWizardOptions>().BindConfiguration("General");
builder.Services.AddOptions<HttpLoaderOptions>().BindConfiguration("HttpLoader");
builder.Services.AddOptions<FileCacheOptions>().BindConfiguration("FileCache");
builder.Services.AddOptions<FileLoaderOptions>().BindConfiguration("FileLoader");
builder.Services.AddOptions<AzureBlobOptions>().BindConfiguration("Azure");
builder.Services.AddOptions<WatermarkOptions>().BindConfiguration("Watermark");

IImageWizardBuilder imageWizard = builder.Services.AddImageWizard()
                                            .AddImageSharp(i => i
                                                .WithPreProcessing(x =>
                                                {
                                                    x.Image.Mutate(m => m.AutoOrient());
                                                })
                                                .WithPostProcessing(x =>
                                                {
                                                    IOptions<ImageWizardAppOptions> options = x.ProcessingContext
                                                                                               .ServiceProvider.GetRequiredService<IOptions<ImageWizardAppOptions>>();

                                                    if (options.Value.UseWebP)
                                                    {
                                                        if (x.ImageFormat is JpegFormat)
                                                        {
                                                            x.ImageFormat = new WebPFormat() { Lossless = false };
                                                        }
                                                        else if (x.ImageFormat is PngFormat)
                                                        {
                                                            x.ImageFormat = new WebPFormat() { Lossless = true };
                                                        }
                                                    }

                                                    if (options.Value.AddMetadata)
                                                    {
                                                        x.Image.Metadata.ExifProfile = new ExifProfile();
                                                        x.Image.Metadata.ExifProfile.SetValue(ExifTag.Copyright, options.Value.MetadataCopyright);
                                                    }
                                                }))
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

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddHttpsRedirection(x => x.HttpsPort = 443);

var app = builder.Build();

var options = app.Services.GetRequiredService<IOptions<ImageWizardAppOptions>>();
var env = app.Services.GetRequiredService<IHostEnvironment>();

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
await app.RunAsync();
