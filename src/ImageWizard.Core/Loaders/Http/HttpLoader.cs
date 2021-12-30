using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.ImageLoaders.Http;
using ImageWizard.Core.StreamPooling;
using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
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

namespace ImageWizard.ImageLoaders
{
    /// <summary>
    /// HttpLoader
    /// </summary>
    public class HttpLoader : DataLoaderBase
    {
        public HttpLoader(
                    HttpClient httpClient, 
                    IOptions<ImageWizardOptions> imageWizardOptions,
                    IOptions<HttpLoaderOptions> options,
                    IHttpContextAccessor httpContextAccessor)
        {
            HttpClient = httpClient;
            HttpContextAccessor = httpContextAccessor;

            ImageWizardOptions = imageWizardOptions.Value;
            Options = options.Value;

            foreach (HttpHeaderItem header in options.Value.Headers)
            {
                HttpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
            }
        }

        /// <summary>
        /// ImageWizardOptions
        /// </summary>
        private ImageWizardOptions ImageWizardOptions { get; }

        /// <summary>
        /// Options
        /// </summary>
        private HttpLoaderOptions Options { get; }

        /// <summary>
        /// HttpClient
        /// </summary>
        private HttpClient HttpClient { get; }

        /// <summary>
        /// HttpContextAccessor
        /// </summary>
        private IHttpContextAccessor HttpContextAccessor { get; }

        public override DataLoaderRefreshMode RefreshMode => Options.RefreshMode;

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override async Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedImage)
        {
            Uri sourceUri;

            //is relative url?
            if (Regex.Match(source, "^https?://", RegexOptions.Compiled).Success == false)
            {
                if(string.IsNullOrWhiteSpace(Options.DefaultBaseUrl) == false)
                {
                    sourceUri = new Uri($"{Options.DefaultBaseUrl.TrimEnd('/')}/{source}");
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

                if (Options.AllowAbsoluteUrls == false)
                {
                    throw new Exception("Absolute urls are not allowed.");
                }

                //check allowed hosts
                if (Options.AllowedHosts.Any())
                {
                    if (Options.AllowedHosts.Any(x => string.Compare(x, sourceUri.Host, true) == 0) == false)
                    {
                        throw new Exception($"Not allowed hosts is used: {sourceUri.Host}");
                    }
                }
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, sourceUri);
            request.AddUserAgentHeader();

            if (existingCachedImage != null)
            {
                if (existingCachedImage.Metadata.Cache.ETag != null)
                {
                    request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue($"\"{existingCachedImage.Metadata.Cache.ETag}\""));
                }
            }

            HttpResponseMessage response = await HttpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotModified)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            byte[] data = await response.Content.ReadAsByteArrayAsync();
            string? mimeType = response.Content.Headers.ContentType?.MediaType;
            bool useStreaming = ImageWizardOptions.StreamingMimeTypes.Any(x => x.Equals(mimeType, StringComparison.OrdinalIgnoreCase));

            if (mimeType == null)
            {
                throw new Exception("no content-type available");
            }

            return new OriginalData(mimeType, data, new CacheSettings(response));
        }
    }
}