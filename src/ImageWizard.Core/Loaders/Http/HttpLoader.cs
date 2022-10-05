// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace ImageWizard.Loaders;

/// <summary>
/// HttpLoader
/// </summary>
public class HttpLoader : HttpLoaderBase<HttpLoaderOptions>
{
    private static readonly Regex AbsoluteUrlRegex = new Regex("^https?://", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public HttpLoader(
                HttpClient client,
                IStreamPool streamPool,
                ILogger<HttpLoader> logger,
                IOptions<HttpLoaderOptions> options,
                IHttpContextAccessor httpContextAccessor)
        : base(client, streamPool, logger, options)
    {
        HttpContextAccessor = httpContextAccessor;

        foreach (HttpHeaderItem header in options.Value.Headers)
        {
            client.DefaultRequestHeaders.Add(header.Name, header.Value);
        }
    }

    /// <summary>
    /// HttpContextAccessor
    /// </summary>
    private IHttpContextAccessor HttpContextAccessor { get; }

    protected override Task<Uri?> CreateRequestUrl(string source)
    {
        Uri sourceUri;

        //is relative url?
        if (AbsoluteUrlRegex.Match(source).Success == false)
        {
            if (string.IsNullOrWhiteSpace(Options.Value.DefaultBaseUrl) == false)
            {
                sourceUri = new Uri($"{Options.Value.DefaultBaseUrl.TrimEnd('/')}/{source}");
            }
            else
            {
                //create absolute url
                sourceUri = new Uri($"{HttpContextAccessor.HttpContext!.Request.Scheme}://{HttpContextAccessor.HttpContext.Request.Host}/{source}");
            }
        }
        else //absolute url
        {
            sourceUri = new Uri(source);

            if (Options.Value.AllowAbsoluteUrls == false)
            {
                throw new Exception("Absolute urls are not allowed.");
            }

            //check allowed hosts
            if (Options.Value.AllowedHosts.Any())
            {
                if (Options.Value.AllowedHosts.Any(x => string.Equals(x, sourceUri.Host, StringComparison.OrdinalIgnoreCase)) == false)
                {
                    throw new Exception($"Not allowed hosts is used: {sourceUri.Host}");
                }
            }
        }

        return Task.FromResult<Uri?>(sourceUri);
    }
}
