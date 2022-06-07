// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageWizard.Loaders;

/// <summary>
/// YouTubeLoader
/// </summary>
public class YouTubeLoader : HttpLoaderBase<YouTubeOptions>
{
    public YouTubeLoader(
        HttpClient client, 
        IStreamPool streamPool, 
        ILogger<YouTubeLoader> logger,
        IOptions<YouTubeOptions> options)
        : base(client, streamPool, logger, options)
    {
    }

    protected override Task<Uri?> CreateRequestUrl(string source)
    {
        return Task.FromResult<Uri?>(new Uri($"https://i.ytimg.com/vi/{source}/maxresdefault.jpg"));
    }
}    
