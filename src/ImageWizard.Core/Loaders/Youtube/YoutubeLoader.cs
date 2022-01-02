using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageWizard.Loaders
{
    /// <summary>
    /// YoutubeLoader
    /// </summary>
    public class YouTubeLoader : DataLoaderBase<YouTubeOptions>
    {
        public YouTubeLoader(HttpClient client, IOptions<YouTubeOptions> options)
            : base(options)
        {
            Client = client;
        }

        /// <summary>
        /// Client
        /// </summary>
        private HttpClient Client { get; }

        public override async Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedImage)
        {
            string url = $"https://i.ytimg.com/vi/{source}/maxresdefault.jpg";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.AddUserAgentHeader();

            if (existingCachedImage != null)
            {
                if (existingCachedImage.Metadata.Cache.ETag != null)
                {
                    request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue($"\"{existingCachedImage.Metadata.Cache.ETag}\""));
                }
            }

            HttpResponseMessage response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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
