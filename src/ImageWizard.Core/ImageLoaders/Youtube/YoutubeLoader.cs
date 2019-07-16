using ImageWizard.ImageLoaders;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageLoaders.Youtube
{
    /// <summary>
    /// YoutubeLoader
    /// </summary>
    public class YoutubeLoader : IImageLoader
    {
        public YoutubeLoader(HttpClient client)
        {
            Client = client;
        }

        /// <summary>
        /// Client
        /// </summary>
        private HttpClient Client { get; }

        /// <summary>
        /// DeliveryType
        /// </summary>
        public string DeliveryType => "youtube";


        public async Task<OriginalImage> GetAsync(string requestUri)
        {
            string url = $"https://i.ytimg.com/vi/{requestUri}/maxresdefault.jpg";

            HttpResponseMessage response = await Client.GetAsync(url);

            byte[] data = await response.Content.ReadAsByteArrayAsync();

            return new OriginalImage(requestUri, response.Content.Headers.ContentType.MediaType, data);
        }
    }
}
