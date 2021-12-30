using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageLoaders.Gravatars
{
    public class GravatarLoader : DataLoaderBase
    {
        public GravatarLoader(HttpClient client, IOptions<GravatarOptions> options)
        {
            Client = client;
            Options = options.Value;
        }

        /// <summary>
        /// Options
        /// </summary>
        private GravatarOptions Options { get; }

        /// <summary>
        /// Client
        /// </summary>
        private HttpClient Client { get; }

        public override DataLoaderRefreshMode RefreshMode => Options.RefreshMode;

        public override async Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedImage)
        {
            string url = $"https://www.gravatar.com/avatar/{source}?size=512";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.AddUserAgentHeader();

            HttpResponseMessage response = await Client.SendAsync(request);

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
