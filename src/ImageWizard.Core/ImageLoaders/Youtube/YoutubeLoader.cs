using ImageWizard.Core.Types;
using ImageWizard.ImageLoaders;
using ImageWizard.Services.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageLoaders.Youtube
{
    /// <summary>
    /// YoutubeLoader
    /// </summary>
    public class YouTubeLoader : ImageLoaderBase
    {
        public YouTubeLoader(HttpClient client, IOptions<YouTubeOptions> options)
        {
            Client = client;
            Options = options.Value;
        }

        /// <summary>
        /// Options
        /// </summary>
        private YouTubeOptions Options { get; }

        /// <summary>
        /// Client
        /// </summary>
        private HttpClient Client { get; }

        public override ImageLoaderRefreshMode RefreshMode => Options.RefreshMode;

        public override async Task<OriginalData?> GetAsync(string source, ICachedImage? existingCachedImage)
        {
            string url = $"https://i.ytimg.com/vi/{source}/maxresdefault.jpg";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            if (existingCachedImage != null)
            {
                if (existingCachedImage.Metadata.Cache.ETag != null)
                {
                    request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue($"\"{existingCachedImage.Metadata.Cache.ETag}\""));
                }
            }

            HttpResponseMessage response = await Client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotModified)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            byte[] data = await response.Content.ReadAsByteArrayAsync();

            string? mimeType = response.Content.Headers.ContentType?.MediaType;

            if (mimeType == null)
            {
                throw new Exception("no content-type available");
            }

            return new OriginalData(mimeType, data, new CacheSettings(response));
        }
    }
}
