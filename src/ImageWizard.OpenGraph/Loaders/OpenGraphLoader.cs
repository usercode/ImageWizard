using ImageWizard.Loaders;
using Microsoft.Extensions.Options;
using OpenGraphNet;
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
    /// OpenGraphLoader
    /// </summary>
    public class OpenGraphLoader : HttpLoaderBase<OpenGraphOptions>
    {
        public OpenGraphLoader(HttpClient client, IOptions<OpenGraphOptions> options)
            : base(client, options)
        {
        }

        protected override async Task<Uri> CreateRequestUrl(string source)
        {
            OpenGraph result = await OpenGraph.ParseUrlAsync(source);

            return result.Image;
        }
    }    
}
