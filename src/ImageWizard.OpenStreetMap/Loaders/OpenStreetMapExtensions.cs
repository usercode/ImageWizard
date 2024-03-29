﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.OpenStreetMap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace ImageWizard;

public static class OpenStreetMapExtensions
{
    public static IImageWizardBuilder AddOpenStreetMapLoader(this IImageWizardBuilder wizardConfiguration, Action<OpenStreetMapOptions>? options = null)
    {
        if (options != null)
        {
            wizardConfiguration.Services.Configure(options);
        }
        wizardConfiguration.Services.AddHttpClient<OpenStreetMapLoader>();
        wizardConfiguration.LoaderManager.Register<OpenStreetMapLoader>("openstreetmap");

        return wizardConfiguration;
    }

    public static IEndpointConventionBuilder MapOpenStreetMap(this IImageWizardEndpointBuilder endpoints)
    {
        return endpoints.MapGet("osm/{**path}", async (
            string path, 
            HttpClient client, 
            HttpResponse response,
            IOptions<OpenStreetMapOptions> options) => 
        {
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Gecko", "20100101"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Firefox", "100.0"));

            HttpResponseMessage res = await client.GetAsync($"{options.Value.Path.TrimEnd('/')}/{path}");

            res.EnsureSuccessStatusCode();

            byte[] imageBuffer = await res.Content.ReadAsByteArrayAsync();

            if (res.Headers.CacheControl != null)
            {
                response.Headers.CacheControl = new Microsoft.Extensions.Primitives.StringValues(res.Headers.CacheControl.ToString());
            }

            return Results.File(imageBuffer, MimeTypes.Png);
        });
    }
}
