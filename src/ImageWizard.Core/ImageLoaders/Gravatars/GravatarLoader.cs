using ImageWizard.ImageLoaders;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageLoaders.Gravatars
{
    public class GravatarLoader : IImageLoader
    {
        public GravatarLoader(HttpClient client)
        {
            Client = client;
        }

        /// <summary>
        /// Client
        /// </summary>
        private HttpClient Client { get; }

        public async Task<OriginalImage> GetAsync(string requestUri)
        {
            string url = $"https://www.gravatar.com/avatar/{requestUri}?size=512";

            HttpResponseMessage response = await Client.GetAsync(url);

            byte[] data = await response.Content.ReadAsByteArrayAsync();

            return new OriginalImage(requestUri, response.Content.Headers.ContentType.MediaType, data);
        }
    }
}
