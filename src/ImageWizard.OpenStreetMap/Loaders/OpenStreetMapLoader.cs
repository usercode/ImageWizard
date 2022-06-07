// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.OpenStreetMap;

class OpenStreetMapLoader : HttpLoaderBase<OpenStreetMapOptions>
{
    public OpenStreetMapLoader(
        HttpClient client, 
        IStreamPool streamPool, 
        ILogger<OpenStreetMapLoader> logger,
        IOptions<OpenStreetMapOptions> options)
        : base(client, streamPool, logger, options)    
    {
    }

    protected override Task<Uri?> CreateRequestUrl(string source)
    {
        return Task.FromResult<Uri?>(new Uri($"{Options.Value.Path.TrimEnd('/')}/{source}.png"));
    }
}
