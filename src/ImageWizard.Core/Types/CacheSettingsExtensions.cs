// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    public static class CacheSettingsExtensions
    {
        public static CacheSettings ApplyHttpResponse(this CacheSettings cacheSettings, HttpResponseMessage response)
        {
            if (response.Headers.CacheControl != null)
            {
                cacheSettings.NoStore = response.Headers.CacheControl.NoStore;
                cacheSettings.NoCache = response.Headers.CacheControl.NoCache;

                if (response.Headers.CacheControl.MaxAge != null)
                {
                    cacheSettings.Expires = DateTime.UtcNow.Add(response.Headers.CacheControl.MaxAge.Value);
                }
                else
                {
                    if (response.Content.Headers.Expires != null)
                    {
                        cacheSettings.Expires = response.Content.Headers.Expires.Value.UtcDateTime;
                    }
                }
            }

            //ETag
            if (response.Headers.ETag != null)
            {
                cacheSettings.ETag = response.Headers.ETag.GetTagUnquoted();
            }

            return cacheSettings;
        }

        public static CacheSettings ApplyLoaderOptions(this CacheSettings cacheSettings, LoaderOptions options)
        {
            if (options.CacheControlMaxAge != null)
            {
                cacheSettings.Expires = DateTime.UtcNow.Add(options.CacheControlMaxAge.Value);
            }

            return cacheSettings;
        }
    }
}
