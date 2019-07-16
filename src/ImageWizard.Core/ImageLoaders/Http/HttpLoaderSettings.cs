﻿using ImageWizard.Core.ImageLoaders.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders
{
    /// <summary>
    /// HttpLoaderSettings
    /// </summary>
    public class HttpLoaderSettings
    {
        public HttpLoaderSettings()
        {
            Headers = new List<HttpHeaderItem>();
        }

        /// <summary>
        /// Headers
        /// </summary>
        public ICollection<HttpHeaderItem> Headers { get; }

        public HttpLoaderSettings SetHeader(string name, string value)
        {
            Headers.Add(new HttpHeaderItem(name, value));

            return this;
        }
    }
}
