// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

    protected override ValueTask<Uri?> CreateRequestUrl(string source)
    {
        return ValueTask.FromResult<Uri?>(new Uri($"{Options.Value.Path.TrimEnd('/')}/{source}.png"));
    }
}
