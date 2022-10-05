// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageWizard.Loaders;

/// <summary>
/// GravatarLoader
/// </summary>
public class GravatarLoader : HttpLoaderBase<GravatarOptions>
{
    public GravatarLoader(
        HttpClient client, 
        IStreamPool streamPool, 
        ILogger<GravatarLoader> logger,
        IOptions<GravatarOptions> options)
        : base(client, streamPool, logger, options)
    {
    }

    protected override Task<Uri?> CreateRequestUrl(string source)
    {
        return Task.FromResult<Uri?>(new Uri($"https://www.gravatar.com/avatar/{source}?size=512"));
    }
}
