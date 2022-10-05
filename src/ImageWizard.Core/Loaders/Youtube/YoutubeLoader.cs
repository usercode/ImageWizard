// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
