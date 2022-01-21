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
    /// <summary>
    /// GravatarLoader
    /// </summary>
    public class GravatarLoader : HttpLoaderBase<GravatarOptions>
    {
        public GravatarLoader(HttpClient client, IOptions<GravatarOptions> options)
            : base(client, options)
        {
        }

        protected override Uri CreateRequestUrl(string source)
        {
            return new Uri($"https://www.gravatar.com/avatar/{source}?size=512");
        }
    }
}
