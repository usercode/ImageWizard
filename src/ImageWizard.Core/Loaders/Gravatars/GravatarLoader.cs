using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Loaders
{
    public class GravatarLoader : DataLoaderBase<GravatarOptions>
    {
        public GravatarLoader(HttpClient client, IOptions<GravatarOptions> options)
            : base(options)
        {
            Client = client;
        }

        /// <summary>
        /// Client
        /// </summary>
        private HttpClient Client { get; }

        public override async Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedData)
        {
            string url = $"https://www.gravatar.com/avatar/{source}?size=512";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.AddUserAgentHeader();

            if (existingCachedData != null)
            {
                if (existingCachedData.Metadata.Cache.ETag != null)
                {
                    request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue($"\"{existingCachedData.Metadata.Cache.ETag}\""));
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
