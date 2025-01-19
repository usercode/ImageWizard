// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

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
    protected abstract ValueTask<Uri?> CreateRequestUrl(string source);

    /// <summary>
    /// GetAsync
    /// </summary>
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
            Logger.LogTrace("Content not modified. {Url}", url);

            return LoaderResult.NotModified();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Logger.LogError("Content not found. {Url}", url);

            return LoaderResult.NotFound();
        }

        if (response.IsSuccessStatusCode == false || mimeType == null)
        {
            Logger.LogError("Couldn't fetch content. (Status code: {StatusCode}) {Url}", response.StatusCode, url);

            return LoaderResult.Failed();
        }

        long? contentLength = response.Content.Headers.ContentLength;
        long maxContentLength = Options.Value.MaxLoaderSourceLength;

        //check content length by header
        if (contentLength > maxContentLength)
        {
            Logger.LogError("Content is too large. (Based on HTTP header) {Url}", url);

            return LoaderResult.Failed();
        }

        using Stream sourceStream = await response.Content.ReadAsStreamAsync();

        //copy to MemoryStream
        Stream mem = StreamPool.GetStream();

        //check content length by download
        if (await sourceStream.TryCopyToAsync(mem, maxContentLength) == false)
        {
            Logger.LogError("Content is too large. (Download was cancelled.) {Url}", url);

            mem.Dispose();

            return LoaderResult.Failed();
        }

        mem.Seek(0, SeekOrigin.Begin);

        return LoaderResult.Success(new OriginalData(mimeType, mem, new CacheSettings()
                                                                                    .ApplyHttpResponse(response)
                                                                                    .ApplyLoaderOptions(Options.Value)));
    }
}
