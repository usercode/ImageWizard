// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageWizard.Loaders;

/// <summary>
/// HttpLoaderBase
/// </summary>
public abstract class HttpLoaderBase<TOptions> : Loader<TOptions>
     where TOptions : LoaderOptions
{
    public HttpLoaderBase(HttpClient client, IOptions<TOptions> options)
        : base(options)
    {
        Client = client;
    }

    /// <summary>
    /// Client
    /// </summary>
    protected HttpClient Client { get; }

    /// <summary>
    /// CreateRequestUrl
    /// </summary>
    /// <returns></returns>
    protected abstract Task<Uri> CreateRequestUrl(string source);

    /// <summary>
    /// GetAsync
    /// </summary>
    /// <param name="source"></param>
    /// <param name="existingCachedData"></param>
    /// <returns></returns>
    public override async Task<LoaderResult> GetAsync(string source, ICachedData? existingCachedData)
    {
        Uri url = await CreateRequestUrl(source);

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
        request.SetUserAgentHeader();
        request.SetIfNoneMatch(existingCachedData);

        HttpResponseMessage response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        string? mimeType = response.Content.Headers.ContentType?.MediaType;

        if (response.StatusCode == HttpStatusCode.NotModified)
        {
            response.Dispose();

            return LoaderResult.NotModified();
        }

        if (response.IsSuccessStatusCode == false || mimeType == null)
        {
            response.Dispose();

            return LoaderResult.Failed();
        }

        Stream data = await response.Content.ReadAsStreamAsync();

        return LoaderResult.Success(new HttpOriginalData(response, mimeType, data, new CacheSettings()
                                                                                    .ApplyHttpResponse(response)
                                                                                    .ApplyLoaderOptions(Options.Value)));
    }
}
