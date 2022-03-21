// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.Options;
using OpenGraphNet;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageWizard.Loaders;

/// <summary>
/// OpenGraphLoader
/// </summary>
public class OpenGraphLoader : HttpLoaderBase<OpenGraphOptions>
{
    public OpenGraphLoader(HttpClient client, IOptions<OpenGraphOptions> options)
        : base(client, options)
    {
    }

    protected override async Task<Uri> CreateRequestUrl(string source)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, source);
        request.SetUserAgentHeader();

        HttpResponseMessage response = await Client.SendAsync(request);

        if (response.IsSuccessStatusCode == false)
        {
            throw new Exception($"Could not load page: {source}");
        }

        string html = await response.Content.ReadAsStringAsync();

        OpenGraph result = OpenGraph.ParseHtml(html);

        return result.Image;
    }
}    
