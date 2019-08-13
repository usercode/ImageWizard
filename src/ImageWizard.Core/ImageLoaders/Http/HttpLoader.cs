using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.ImageLoaders.Http;
using ImageWizard.Core.Types;
using ImageWizard.Filters.ImageFormats;
using ImageWizard.Services.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.ImageLoaders
{
    /// <summary>in
    /// ImageDownloader
    /// </summary>
    public class HttpLoader : IImageLoader
    {
        public HttpLoader(HttpClient httpCLient, IOptions<HttpLoaderSettings> settings, IHttpContextAccessor httpContextAccessor)
        {
            HttpClient = httpCLient;
            Settings = settings;
            HttpContextAccessor = httpContextAccessor;

            foreach (HttpHeaderItem header in Settings.Value.Headers)
            {
                HttpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
            }
        }

        /// <summary>
        /// HttpClient
        /// </summary>
        private HttpClient HttpClient { get; }

        /// <summary>
        /// Settings
        /// </summary>
        public IOptions<HttpLoaderSettings> Settings { get; }

        /// <summary>
        /// HttpContextAccessor
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; }

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<OriginalImage> GetAsync(string requestUri)
        {
            //is relative url?
            if (Regex.Match(requestUri, "^https?://").Success == false)
            {
                requestUri = $"{HttpContextAccessor.HttpContext.Request.Scheme}://{HttpContextAccessor.HttpContext.Request.Host}/{requestUri}";
            }

            HttpResponseMessage response = await HttpClient.GetAsync(requestUri);
            byte[] data = await response.Content.ReadAsByteArrayAsync();

            string mimeType = response.Content.Headers.ContentType?.MediaType;

            if (mimeType == null)
            {
                //mimeType = ImageFormatHelper.GetMimeTypeByExtension(requestUri);

                throw new Exception("no content-type available");
            }

            CacheSettings cacheSettings = new CacheSettings();

            if (response.Headers.CacheControl != null)
            {
                cacheSettings.NoStore = response.Headers.CacheControl.NoStore;
                cacheSettings.NoCache = response.Headers.CacheControl.NoCache;
                cacheSettings.MaxAge = response.Headers.CacheControl.MaxAge;
            }

            cacheSettings.ETag = response.Headers.ETag?.Tag;

            return new OriginalImage(requestUri, mimeType, data, cacheSettings);
        }
    }
}
