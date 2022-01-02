using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.Loaders
{
    /// <summary>
    /// HttpLoader
    /// </summary>
    public class HttpLoader : DataLoaderBase<HttpLoaderOptions>
    {
        private static readonly Regex AbsoluteUrlRegex = new Regex("^https?://", RegexOptions.Compiled);

        public HttpLoader(
                    HttpClient httpClient,
                    IOptions<HttpLoaderOptions> options,
                    IHttpContextAccessor httpContextAccessor)
            :base(options)
        {
            HttpClient = httpClient;
            HttpContextAccessor = httpContextAccessor;

            foreach (HttpHeaderItem header in options.Value.Headers)
            {
                HttpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
            }
        }

        /// <summary>
        /// HttpClient
        /// </summary>
        private HttpClient HttpClient { get; }

        /// <summary>
        /// HttpContextAccessor
        /// </summary>
        private IHttpContextAccessor HttpContextAccessor { get; }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override async Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedData)
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

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, sourceUri);
            request.AddUserAgentHeader();

            if (existingCachedData != null)
            {
                if (existingCachedData.Metadata.Cache.ETag != null)
                {
                    request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue($"\"{existingCachedData.Metadata.Cache.ETag}\""));
                }
            }

            HttpResponseMessage response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            string? mimeType = response.Content.Headers.ContentType?.MediaType;

            if (response.IsSuccessStatusCode == false 
                            || mimeType == null
                            || response.StatusCode == HttpStatusCode.NotModified)
            {
                response.Dispose();

                return null;
            }

            Stream data = await response.Content.ReadAsStreamAsync();

            return new HttpOriginalData(response, mimeType, data, new CacheSettings(response));
        }
    }
}