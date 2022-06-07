﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Logging;
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
    public HttpLoaderBase(
        HttpClient client, 
        IStreamPool streamPool, 
        ILogger logger,
        IOptions<TOptions> options)
        : base(options)
    {
        Client = client; 
        StreamPool = streamPool;
        Logger = logger;
    }

    /// <summary>
    /// Client
    /// </summary>
    protected HttpClient Client { get; }

    /// <summary>
    /// StreamPool
    /// </summary>
    protected IStreamPool StreamPool { get; }

    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// CreateRequestUrl
    /// </summary>
    /// <returns></returns>
    protected abstract Task<Uri?> CreateRequestUrl(string source);

    /// <summary>
    /// GetAsync
    /// </summary>
    /// <param name="source"></param>
    /// <param name="existingCachedData"></param>
    /// <returns></returns>
    public override async Task<LoaderResult> GetAsync(string source, ICachedData? existingCachedData)
    {
        Uri? url = await CreateRequestUrl(source);

        if (url == null)
        {
            Logger.LogError("Couldn't create the url for source '{source}'.", source);

            return LoaderResult.Failed();
        }

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
        request.SetUserAgentHeader();
        request.SetIfNoneMatch(existingCachedData);

        using HttpResponseMessage response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        string? mimeType = response.Content.Headers.ContentType?.MediaType;

        if (response.StatusCode == HttpStatusCode.NotModified)
        {
            return LoaderResult.NotModified();
        }

        if (response.IsSuccessStatusCode == false || mimeType == null)
        {
            Logger.LogError("Couldn't fetch content. (Status code: {StatusCode})", response.StatusCode);

            return LoaderResult.Failed();
        }

        long? contentLength = response.Content.Headers.ContentLength;
        long maxContentLength = Options.Value.MaxLoaderSourceLength;

        //check content length by header
        if (contentLength > maxContentLength)
        {
            Logger.LogError("Content is too large. (Based on HTTP header)");

            return LoaderResult.Failed();
        }

        using Stream sourceStream = await response.Content.ReadAsStreamAsync();

        //copy to MemoryStream
        Stream mem = StreamPool.GetStream();

        //check content length by download
        if (await sourceStream.TryCopyToAsync(mem, maxContentLength) == false)
        {
            Logger.LogError("Content is too large. (Download was cancelled.)");

            mem.Dispose();

            return LoaderResult.Failed();
        }

        mem.Seek(0, SeekOrigin.Begin);

        return LoaderResult.Success(new OriginalData(mimeType, mem, new CacheSettings()
                                                                                    .ApplyHttpResponse(response)
                                                                                    .ApplyLoaderOptions(Options.Value)));
    }
}
