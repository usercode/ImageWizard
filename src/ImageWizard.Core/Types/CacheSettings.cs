using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ImageWizard.Core.Types
{
    /// <summary>
    /// CacheSettings
    /// </summary>
    public class CacheSettings
    {
        public CacheSettings()
        {

        }

        public CacheSettings(HttpResponseMessage response)
        {
            if (response.Headers.CacheControl != null)
            {
                NoStore = response.Headers.CacheControl.NoStore;
                NoCache = response.Headers.CacheControl.NoCache;

                if (response.Headers.CacheControl.MaxAge != null)
                {
                    Expires = DateTime.UtcNow.Add(response.Headers.CacheControl.MaxAge.Value);
                }
                else
                {
                    if (response.Content.Headers.Expires != null)
                    {
                        Expires = response.Content.Headers.Expires.Value.UtcDateTime;
                    }
                }
            }

            //ETag
            if (response.Headers.ETag != null)
            {
                ETag = response.Headers.ETag.GetTagUnquoted();
            }
        }

        public bool NoCache { get; set; }
        public bool NoStore { get; set; }
        public string? ETag { get; set; }
        public DateTime? Expires { get; set; }

    }
}
