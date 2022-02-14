using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Loaders
{
    /// <summary>
    /// HttpLoaderSettings
    /// </summary>
    public class HttpLoaderOptions : LoaderOptions
    {
        public HttpLoaderOptions()
        {
            AllowAbsoluteUrls = false;
            AllowedHosts = Array.Empty<string>();
            RefreshMode = LoaderRefreshMode.None;
            Headers = new List<HttpHeaderItem>();
        }

        /// <summary>
        /// Headers
        /// </summary>
        public ICollection<HttpHeaderItem> Headers { get; }

        /// <summary>
        /// Used for relative urls.
        /// </summary>
        public string? DefaultBaseUrl { get; set; }

        /// <summary>
        /// AllowAbsoluteUrls
        /// </summary>
        public bool AllowAbsoluteUrls { get; set; }

        /// <summary>
        /// AllowedHosts
        /// </summary>
        public string[] AllowedHosts { get; set; }

        public HttpLoaderOptions SetHeader(string name, string value)
        {
            Headers.Add(new HttpHeaderItem(name, value));

            return this;
        }
    }
}
