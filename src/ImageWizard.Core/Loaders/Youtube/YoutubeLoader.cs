using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageWizard.Loaders
{
    /// <summary>
    /// YouTubeLoader
    /// </summary>
    public class YouTubeLoader : HttpLoaderBase<YouTubeOptions>
    {
        public YouTubeLoader(HttpClient client, IOptions<YouTubeOptions> options)
            : base(client, options)
        {
        }

        protected override Task<Uri> CreateRequestUrl(string source)
        {
            return Task.FromResult(new Uri($"https://i.ytimg.com/vi/{source}/maxresdefault.jpg"));
        }
    }    
}
