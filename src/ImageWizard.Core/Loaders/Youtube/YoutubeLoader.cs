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

    protected override ValueTask<Uri?> CreateRequestUrl(string source)
    {
        return ValueTask.FromResult<Uri?>(new Uri(source));
    }

    public override async Task<LoaderResult> GetAsync(string source, ICachedData? existingCachedData)
    {
        string[] quality = [
                            "maxresdefault.jpg", //1920 x 1080   
                            "hq720.jpg",        //1280 x 720 
                            "sddefault.jpg", //640 x 480 
                            "hqdefault.jpg", //480 x 360                            
                        ];

        for (int i = 0; i < quality.Length; i++)
        {
            LoaderResult result = await base.GetAsync($"https://i.ytimg.com/vi/{source}/{quality[i]}", existingCachedData);

            if (result.State == LoaderResultState.Success)
            {
                return result;
            }
        }

        return LoaderResult.NotFound();
    }
}    
